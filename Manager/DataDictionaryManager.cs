using Common;
using Domain;
using IManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Manager.DataDictionaryManager.DataDictionaryDataChangedArgs;

namespace Manager
{
    public class DataDictionaryManager : IDataDictionaryManager,IDependency
    {
        /// <summary>
        /// 数据字典内容变更事件
        /// </summary>
        public static event EventHandler<DataDictionaryDataChangedArgs> DataDictionaryDataChanged;

        /// <summary>
        /// 数据字典数据变更用参数
        /// </summary>
        public class DataDictionaryDataChangedArgs : EventArgs
        {
            public bool HasValue
            {
                get
                {
                    if (fdataDictionaries.Count == 0 && dataDictionary.Count == 0)
                    {
                        return false;
                    }
                    return true;
                }
            }

            /// <summary>
            /// 子级字典变更列表
            /// </summary>
            private readonly List<DataDictionary> dataDictionary = new List<DataDictionary>();

            /// <summary>
            /// 父级字典变更列表
            /// </summary>
            private readonly List<FDataDictionary> fDataDictionary = new List<FDataDictionary>();

            /// <summary>
            /// 字典变更类型
            /// </summary>
            public ChangeType? DictionaryChangeType { get; set; }

            /// <summary>
            /// 添加变更的实体
            /// </summary>
            public void Add(FDataDictionary entity)
            {
                fDataDictionary.Add(entity);
            }

            /// <summary>
            /// 添加变更的实体
            /// </summary>
            public void Add(DataDictionary entity)
            {
                dataDictionary.Add(entity);
            }

            public IEnumerable<FDataDictionary> GetFDataDictionaries()
            {
                return fDataDictionary;
            }
            public IEnumerable<DataDictionary> GetDataDictionaries()
            {
                return dataDictionary;
            }

            /// <summary>
            /// 字典变更类型枚举
            /// </summary>
            public enum ChangeType
            {
                Add,
                Edit,
                Delete
            }
        }

        #region 数据字典页面 方法
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="entity">参数实体对象</param>
        public void Add(DataDictionaryAddParams entity)
        {
            var args = new DataDictionaryDataChangedArgs
            {
                DictionaryChangeType = ChangeType.Add
            };
            using (var db = new ShopEntities())
            {
                //判断可空类型是否有值，如果有值返回true反之false
                if (entity.FID.HasValue)
                {
                    int value;
                    if (!int.TryParse(entity.Value, out value))
                    {
                        throw new OperateException("子级字典值应是数值");
                    }
                    if (db.DataDictionary.Where(p => p.Value == value && p.FDictionaryID == entity.FID).Count() > 0)
                    {
                        throw new OperateException("值重复");
                    }
                    var dataDictionary = new DataDictionary
                    {
                        FDictionaryID = entity.FID.Value,
                        Color = entity.Color,
                        Icon = entity.Icon,
                        Key = entity.Key,
                        Custom = entity.Custom,
                        Value = value,
                        Name = entity.Name
                    };
                    db.DataDictionary.Add(dataDictionary);
                    args.Add(dataDictionary);
                }
                else
                {
                    if (db.FDataDictionary.Where(p => p.Argument.Equals(entity.Value)).Count() > 0)
                    {
                        throw new OperateException("值重复");
                    }
                    var fDataDictionary = new FDataDictionary
                    {
                        Name = entity.Key,
                        Argument = entity.Value,
                    };
                    db.FDataDictionary.Add(fDataDictionary);
                    args.Add(fDataDictionary);
                }
                if (!(db.SaveChanges() > 0))
                {
                    throw new OperateException("数据库错误,没有数据变化");
                }
            }
            try
            {
                Task.Run(() => DataDictionaryDataChanged?.Invoke(null, args));
            }
            catch
            {
            }
            InitList();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete(DeleteDataDictionaryParams @params)
        {
            if (@params.FID.HasValue)
            {
                DeleteC(@params.FID.Value, @params.ID);
            }
            else
            {
                DeleteF(@params.ID);
            }
            InitList();
        }

        /// <summary>
        /// 删除子级
        /// </summary>
        /// <param name="fid">父级ID</param>
        /// <param name="id">子级ID</param>
        public void DeleteC(int fid, int id)
        {
            var args = new DataDictionaryDataChangedArgs
            {
                DictionaryChangeType = ChangeType.Delete
            };
            using (var db = new ShopEntities())
            {
                var entity = db.DataDictionary.FirstOrDefault(p => p.FDictionaryID == fid && p.DictionaryID == id) ?? throw new OperateException("数据库中不存在此子级");
                args.Add(entity);
                db.DataDictionary.Remove(entity);
                db.SaveChanges();
            }
            try
            {
                Task.Run(() => DataDictionaryDataChanged?.Invoke(null, args));
            }
            catch
            {
            }
        }

        /// <summary>
        /// 删除父级
        /// </summary>
        /// <param name="id">父级ID</param>
        public void DeleteF(int id)
        {
            var args = new DataDictionaryDataChangedArgs
            {
                DictionaryChangeType = ChangeType.Delete
            };
            using (var db = new ShopEntities())
            {
                var entity = db.FDataDictionary.FirstOrDefault(p => p.FDictionaryID == id) ?? throw new OperateException("数据库中不存在此父级");
                List<DataDictionary> chirenData = entity.DataDictionary.ToList();
                db.DataDictionary.RemoveRange(chirenData);
                db.FDataDictionary.Remove(entity);
                db.SaveChanges();
                args.Add(entity);
                foreach (var item in chirenData)
                {
                    args.Add(item);
                }
            }
            try
            {
                Task.Run(() => DataDictionaryDataChanged?.Invoke(null, args));
            }
            catch
            {
            }
        }

        /// <summary>
        /// 编辑字典
        /// </summary>
        /// <param name="entity">参数实体对象</param>
        public void Edit(EdtiDataDictionary entity)
        {
            if (entity.fDictionaryID.HasValue)
            {
                var data = new DataDictionary
                {
                    Color = entity.Color,
                    Custom = entity.custom,
                    Key = entity.key,
                    DictionaryID = entity.DictionaryID,
                    FDictionaryID = entity.fDictionaryID.Value,
                    Icon = entity.icon,
                    Name = entity.Name
                };
                try
                {
                    data.Value = int.Parse(entity.value);
                }
                catch (FormatException ex)
                {
                    throw new OperateException("子级字典值应是一个数值", ex);
                }
                Edit(data);
            }
            else
            {
                var data = new FDataDictionary
                {
                    Name = entity.key,
                    Argument = entity.value,
                    FDictionaryID = entity.DictionaryID
                };
                Edit(data);
            }
            InitList();
        }

        /// <summary>
        /// 编辑父级字典
        /// </summary>
        /// <param name="changedDataDictionary">父级实体对象</param>
        public void Edit(FDataDictionary changedDataDictionary)
        {
            var args = new DataDictionaryDataChangedArgs
            {
                DictionaryChangeType = ChangeType.Edit
            };
            using (var db = new ShopEntities())
            {
                var entity = db.FDataDictionary.Where(p => p.FDictionaryID == changedDataDictionary.FDictionaryID).FirstOrDefault() ?? throw new OperateException("指定的字典父级已删除");
                var data = db.FDataDictionary.Where(p => p.Argument == changedDataDictionary.Argument).FirstOrDefault();
                if (entity.Argument != changedDataDictionary.Argument && data != null)
                {
                    throw new OperateException("已存在同值父级");
                }
                db.Entry(entity).State = EntityState.Detached;
                db.Entry(db.FDataDictionary.Attach(changedDataDictionary)).State = EntityState.Modified;
                db.SaveChanges();
                args.Add(changedDataDictionary);
            }
            try
            {
                Task.Run(() => DataDictionaryDataChanged?.Invoke(null, args));
            }
            catch
            {
            }
        }

        /// <summary>
        /// 编辑子级字典
        /// </summary>
        /// <param name="changedDataDictionary">子级实体对象</param>
        public void Edit(DataDictionary changedDataDictionary)
        {
            var args = new DataDictionaryDataChangedArgs
            {
                DictionaryChangeType = ChangeType.Edit
            };
            using (var db = new ShopEntities())
            {
                var entity = db.DataDictionary.Where(p => p.DictionaryID == changedDataDictionary.DictionaryID).FirstOrDefault() ?? throw new OperateException("不存在指定的的子级");
                var dataDictionary = db.DataDictionary.Where(p => p.Value == changedDataDictionary.Value && p.FDictionaryID == entity.FDictionaryID).FirstOrDefault();
                if (entity.Value != changedDataDictionary.Value && dataDictionary != null)
                {
                    throw new OperateException("子级中存在相同值");
                }
                db.Entry(entity).State = EntityState.Detached;
                db.Entry(db.DataDictionary.Attach(changedDataDictionary)).State = EntityState.Modified;
                db.SaveChanges();
                args.Add(changedDataDictionary);
            }
            try
            {
                Task.Run(() => DataDictionaryDataChanged?.Invoke(null, args));
            }
            catch
            {
            }
        }
        /// <summary>
        /// 查询字典数据
        /// </summary>
        /// <returns></returns>
        public object Query()
        {
            return fdataDictionaries.OrderBy(p => p.Argument).Select(p => new
            {
                p.FDictionaryID,
                Key = p.Name,
                Value = p.Argument,
                DataID = p.FDictionaryID,
                children = p.DataDictionary.OrderBy(x => x.Value).Select(l => new
                {
                    DataID = p.FDictionaryID.ToString() + "_" + l.DictionaryID.ToString(),
                    l.FDictionaryID,
                    l.DictionaryID,
                    l.Key,
                    l.Value,
                    l.Name,
                    Icon = l.Icon ?? string.Empty,
                    Color = l.Color ?? string.Empty,
                    Custom = l.Custom ?? string.Empty
                }).ToList()
            });
        }

        /// <summary>
        /// 根据名称查询字典数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Query(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new OperateException("查询目标不能为空");
            }
            object selector(FDataDictionary p) => new
            {
                DataID = p.FDictionaryID,
                p.FDictionaryID,
                Key = p.Name,
                Value = p.Argument,
                children = p.DataDictionary.Select(l => new
                {
                    DataID = p.FDictionaryID.ToString() + "_" + l.DictionaryID.ToString(),
                    l.FDictionaryID,
                    l.DictionaryID,
                    l.Key,
                    l.Value,
                    l.Name,
                    Icon = l.Icon ?? string.Empty,
                    Color = l.Color ?? string.Empty,
                    Custom = l.Custom ?? string.Empty
                }).ToList()
            };
            var global = fdataDictionaries.Where(p => p.Argument.StartsWith("g_") && !p.Name.Contains(name)).Select(selector).ToList();
            var data = fdataDictionaries.Where(p => p.Name.Contains(name) || p.Argument.Contains(name)).Select(selector).ToList();
            data.AddRange(global);            
            data = data.DistinctBy(p => p.GetPropertyValue<int>("DataID")).ToList();
            return data;
        }

        public object Query1()
        {
            return fdataDictionaries.Select(p => new
            {
                Name = p.Name,
                Type = p.Argument,
                Sub = p.DataDictionary.Select(d => new
                {
                    d.Value,
                    Icon = d.Icon ?? string.Empty,
                    Custom = d.Custom ?? string.Empty,
                    Color = d.Color ?? string.Empty,
                    Name = d.Key
                }).ToList()
            }).ToList();
        }

        /// <summary>
        /// 根据枚举值获取子级字典
        /// </summary>
        /// <param name="fID">父级ID</param>
        /// <param name="cID">子级ID</param>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public DataDictionary GetDataDictionary(int fID, int value)
        {
            var fdata = fdataDictionaries.Where(p => p.FDictionaryID == fID).FirstOrDefault();
            if (fdata is null)
            {
                return null;
            }
            return fdata.DataDictionary.Where(p => p.Value == value).FirstOrDefault();
        }
        /// <summary>
        /// 根据子级id查询子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataDictionary GetChilderDataDetail(int id)
        {
            var data = fdataDictionaries.SelectMany(p => p.DataDictionary).Where(p => p.DictionaryID == id).FirstOrDefault();
            return data;
        }
        /// <summary>
        /// 根据子级id查询子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DataDictionary> GetChilderDataList(string ids)
        {
            var data = fdataDictionaries.SelectMany(p => p.DataDictionary).Where(p => ids.Contains("," + p.DictionaryID + ",")).ToList();
            return data;
        }
        #endregion

        /// <summary>
        /// 批量修改 改custom
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool Edits(List<DataDictionary> datas)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var args = new DataDictionaryDataChangedArgs
                {
                    DictionaryChangeType = ChangeType.Edit
                };
                foreach (var item in datas)
                {
                    DataDictionary dataDictionary = db.DataDictionary.FirstOrDefault(s => s.DictionaryID == item.DictionaryID);
                    dataDictionary.Custom = item.Custom;
                    args.Add(dataDictionary);
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
                try
                {
                    Task.Run(() => DataDictionaryDataChanged?.Invoke(null, args));
                }
                catch
                {
                }
                InitList();
                return true;
            }
        }
        public static List<FDataDictionary> fdataDictionaries = new List<FDataDictionary>();
        #region 统一获取数据字典参数
        /**
         * 说明：(在数据库中 Argument、Value 才是唯一的)
         *      1、获取查询父级时 FDataDictionary 对象以 Argument 字段唯一区分
         *      2、获取查询子级时 DataDictionary 对象以 Value 字段唯一区分
         *      3、添加特殊 查询方法时 方法需要 static 化
         * 
         * **/

        /// <summary>
        /// 根据父级Argument  获取子级列表
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <returns></returns>
        public List<DataDictionary> LIST(string Argument)
        {
            FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，LIST：{Argument}");
            return fDataDictionary.DataDictionary.ToList();
        }

        /// <summary>
        /// 根据 父级Argument、子级枚举值  获取子级对象
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="value">子级枚举值</param>
        /// <returns></returns>
        public DataDictionary ITEM(string Argument, int value)
        {
            FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(ITEM)}：{Argument}");
            var dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Value == value) ?? throw new OperateException($"未知异常，{nameof(ITEM)}：{nameof(Argument)}：{Argument}---{nameof(value)}：{value}");
            return dataDictionary;
        }

        /// <summary>
        /// 根据父级Argument，子级名称，获取子级对象
        /// </summary>
        /// <param name="Argument"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataDictionary ITEM(string Argument, string name)
        {
            FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(ITEM)}：{Argument}");
            var dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Key == name) ?? throw new OperateException($"未知异常，{nameof(ITEM)}：{nameof(Argument)}：{Argument}---{nameof(name)}：{name}");
            return dataDictionary;
        }

        /// <summary>
        /// 根据 父级Argument、子级名称、子级枚举值  判断子级枚举值是否正确
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="key">子级名称</param>
        /// <param name="value">子级枚举值</param>
        /// <returns></returns>
        public bool CHECK(string Argument, string key, int value) => VALUE(Argument, key).Equals(value);

        /// <summary>
        /// 根据 父级Argument、子级名称  获取子级枚举值
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="key">子级名称</param>
        /// <returns></returns>
        public int VALUE(string Argument, string key)
        {
            FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(VALUE)}：{Argument}");
            DataDictionary dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Key == key) ?? throw new OperateException($"未知异常，{nameof(VALUE)}：{nameof(Argument)}：{Argument}---{nameof(key)}：{ key}");
            return dataDictionary.Value;
        }

        /// <summary>
        ///  根据父级Argument、子级名称 获取自己枚举值（仅允许g_config使用）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int VALUE_CONFIG(string name)
        {
            string Argument = "g_config";
            FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(VALUE)}：{Argument}");
            DataDictionary dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Name == name) ?? throw new OperateException($"未知异常，{nameof(VALUE)}：{nameof(Argument)}：{Argument}---{nameof(name)}：{ name}");
            return dataDictionary.Value;
        }

        /// <summary>
        /// 获取成员 姓 的拼音
        /// </summary>
        /// <param name="surname">姓</param>
        /// <returns></returns>
        public string GetUserNamePY(string surname)
        {
            if (string.IsNullOrEmpty(surname)) return null;
            var UserNameList = fdataDictionaries.FirstOrDefault(aa => aa.Argument == "user_name")?.DataDictionary;
            if (UserNameList == null) return null;
            return UserNameList.FirstOrDefault(aa => aa.Key == surname)?.Name;
        }

        /// <summary>
        /// 根据 父级Argument、子级名称  获取子级枚Custom
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="key">子级名称</param>
        /// <returns></returns>
        public string GetCustom(string Argument, string key)
        {
            FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(GetCustom)}：{Argument}");
            DataDictionary dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Key == key) ?? throw new OperateException($"未知异常，{nameof(GetCustom)}：{nameof(Argument)}：{Argument}---{nameof(key)}：{ key }");
            return dataDictionary.Custom;
        }

        /// <summary>
        /// 根据 父级Argument、子级枚举值  获取子级枚Custom
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="value">子级名称</param>
        /// <returns></returns>
        public string GetCustom(string Argument, int val)
        {
            FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(GetCustom)}：{Argument}");
            DataDictionary dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Value == val) ?? throw new OperateException($"未知异常，{nameof(GetCustom)}：{nameof(Argument)}：{Argument}---{nameof(val)}：{ val }");
            return dataDictionary.Custom;
        }

        #endregion

        /// <summary>
        /// 网站启动的时候初始化字典
        /// </summary>
        public void InitList()
        {
            using (var db = new ShopEntities())
            {
                fdataDictionaries = db.FDataDictionary.Include("DataDictionary").ToList();
            }
        }
    }
    //public class D
    //{
    //    public static List<FDataDictionary> fdataDictionaries;
    //    #region 统一获取数据字典参数
    //    /**
    //     * 说明：(在数据库中 Argument、Value 才是唯一的)
    //     *      1、获取查询父级时 FDataDictionary 对象以 Argument 字段唯一区分
    //     *      2、获取查询子级时 DataDictionary 对象以 Value 字段唯一区分
    //     *      3、添加特殊 查询方法时 方法需要 static 化
    //     * 
    //     * **/

    //    /// <summary>
    //    /// 根据父级Argument  获取子级列表
    //    /// </summary>
    //    /// <param name="Argument">父级Argument</param>
    //    /// <returns></returns>
    //    public static List<DataDictionary> LIST(string Argument)
    //    {
    //        FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，LIST：{Argument}");
    //        return fDataDictionary.DataDictionary.ToList();
    //    }

    //    /// <summary>
    //    /// 根据 父级Argument、子级枚举值  获取子级对象
    //    /// </summary>
    //    /// <param name="Argument">父级Argument</param>
    //    /// <param name="value">子级枚举值</param>
    //    /// <returns></returns>
    //    public static DataDictionary ITEM(string Argument, int value)
    //    {
    //        FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(ITEM)}：{Argument}");
    //        var dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Value == value) ?? throw new OperateException($"未知异常，{nameof(ITEM)}：{nameof(Argument)}：{Argument}---{nameof(value)}：{value}");
    //        return dataDictionary;
    //    }

    //    /// <summary>
    //    /// 根据父级Argument，子级名称，获取子级对象
    //    /// </summary>
    //    /// <param name="Argument"></param>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    public static DataDictionary ITEM(string Argument, string name)
    //    {
    //        FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(ITEM)}：{Argument}");
    //        var dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Key == name) ?? throw new OperateException($"未知异常，{nameof(ITEM)}：{nameof(Argument)}：{Argument}---{nameof(name)}：{name}");
    //        return dataDictionary;
    //    }

    //    /// <summary>
    //    /// 根据 父级Argument、子级名称、子级枚举值  判断子级枚举值是否正确
    //    /// </summary>
    //    /// <param name="Argument">父级Argument</param>
    //    /// <param name="key">子级名称</param>
    //    /// <param name="value">子级枚举值</param>
    //    /// <returns></returns>
    //    public static bool CHECK(string Argument, string key, int value) => VALUE(Argument, key).Equals(value);

    //    /// <summary>
    //    /// 根据 父级Argument、子级名称  获取子级枚举值
    //    /// </summary>
    //    /// <param name="Argument">父级Argument</param>
    //    /// <param name="key">子级名称</param>
    //    /// <returns></returns>
    //    public static int VALUE(string Argument, string key)
    //    {
    //        FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(VALUE)}：{Argument}");
    //        DataDictionary dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Key == key) ?? throw new OperateException($"未知异常，{nameof(VALUE)}：{nameof(Argument)}：{Argument}---{nameof(key)}：{ key}");
    //        return dataDictionary.Value;
    //    }

    //    /// <summary>
    //    ///  根据父级Argument、子级名称 获取自己枚举值（仅允许g_config使用）
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <returns></returns>
    //    public static int VALUE_CONFIG(string name)
    //    {
    //        string Argument = "g_config";
    //        FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(VALUE)}：{Argument}");
    //        DataDictionary dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Name == name) ?? throw new OperateException($"未知异常，{nameof(VALUE)}：{nameof(Argument)}：{Argument}---{nameof(name)}：{ name}");
    //        return dataDictionary.Value;
    //    }

    //    /// <summary>
    //    /// 获取成员 姓 的拼音
    //    /// </summary>
    //    /// <param name="surname">姓</param>
    //    /// <returns></returns>
    //    public static string GetUserNamePY(string surname)
    //    {
    //        if (string.IsNullOrEmpty(surname)) return null;
    //        var UserNameList = fdataDictionaries.FirstOrDefault(aa => aa.Argument == "user_name")?.DataDictionary;
    //        if (UserNameList == null) return null;
    //        return UserNameList.FirstOrDefault(aa => aa.Key == surname)?.Name;
    //    }

    //    /// <summary>
    //    /// 根据 父级Argument、子级名称  获取子级枚Custom
    //    /// </summary>
    //    /// <param name="Argument">父级Argument</param>
    //    /// <param name="key">子级名称</param>
    //    /// <returns></returns>
    //    public static string GetCustom(string Argument, string key)
    //    {
    //        FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(GetCustom)}：{Argument}");
    //        DataDictionary dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Key == key) ?? throw new OperateException($"未知异常，{nameof(GetCustom)}：{nameof(Argument)}：{Argument}---{nameof(key)}：{ key }");
    //        return dataDictionary.Custom;
    //    }

    //    /// <summary>
    //    /// 根据 父级Argument、子级枚举值  获取子级枚Custom
    //    /// </summary>
    //    /// <param name="Argument">父级Argument</param>
    //    /// <param name="value">子级名称</param>
    //    /// <returns></returns>
    //    public static string GetCustom(string Argument, int val)
    //    {
    //        FDataDictionary fDataDictionary = fdataDictionaries.FirstOrDefault(aa => aa.Argument == Argument) ?? throw new OperateException($"未知异常，{nameof(GetCustom)}：{Argument}");
    //        DataDictionary dataDictionary = fDataDictionary.DataDictionary.FirstOrDefault(aa => aa.Value == val) ?? throw new OperateException($"未知异常，{nameof(GetCustom)}：{nameof(Argument)}：{Argument}---{nameof(val)}：{ val }");
    //        return dataDictionary.Custom;
    //    }

    //    #endregion

    //    /// <summary>
    //    /// 网站启动的时候初始化字典
    //    /// </summary>
    //    public static void InitList()
    //    {
    //        using (var db = new ShopEntities())
    //        {
    //            fdataDictionaries = db.FDataDictionary.Include("DataDictionary").ToList();
    //        }
    //    }
    //}
}
