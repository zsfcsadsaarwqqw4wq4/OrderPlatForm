using Domain;
using IComponent;
using IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class DataDictionaryComponent : IDataDictionaryComponent, IDependencys
    {
        public IDataDictionaryManager IDDM { get; set; }
        #region 数据字典页面 方法
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="entity">参数实体对象</param>
        public void Add(DataDictionaryAddParams entity)
        {
            IDDM.Add(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete(DeleteDataDictionaryParams @params)
        {
            IDDM.Delete(@params);
        }

        /// <summary>
        /// 删除子级
        /// </summary>
        /// <param name="fid">父级ID</param>
        /// <param name="id">子级ID</param>
        public void DeleteC(int fid, int id)
        {
            IDDM.DeleteC(fid, id);
        }

        /// <summary>
        /// 删除父级
        /// </summary>
        /// <param name="id">父级ID</param>
        public void DeleteF(int id)
        {
            IDDM.DeleteF(id);
        }

        /// <summary>
        /// 编辑字典
        /// </summary>
        /// <param name="entity">参数实体对象</param>
        public void Edit(EdtiDataDictionary entity)
        {
            IDDM.Edit(entity);
        }

        /// <summary>
        /// 编辑父级字典
        /// </summary>
        /// <param name="changedDataDictionary">父级实体对象</param>
        public void Edit(FDataDictionary changedDataDictionary)
        {
            IDDM.Edit(changedDataDictionary);
        }

        /// <summary>
        /// 编辑子级字典
        /// </summary>
        /// <param name="changedDataDictionary">子级实体对象</param>
        public void Edit(DataDictionary changedDataDictionary)
        {
            IDDM.Edit(changedDataDictionary);
        }

        /// <summary>
        /// 查询字典数据
        /// </summary>
        /// <returns></returns>
        public object Query()
        {
            return IDDM.Query();
        }

        /// <summary>
        /// 根据名称查询字典数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Query(string name)
        {
            return IDDM.Query(name);
        }

        public object Query1()
        {
            return IDDM.Query1();
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
            return IDDM.GetDataDictionary(fID, value);
        }
        /// <summary>
        /// 根据子级id查询子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataDictionary GetChilderDataDetail(int id)
        {
            return IDDM.GetChilderDataDetail(id);
        }
        /// <summary>
        /// 根据子级id查询子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DataDictionary> GetChilderDataList(string ids)
        {
            return IDDM.GetChilderDataList(ids);
        }

        #endregion
        /// <summary>
        /// 批量修改 改custom
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool Edits(List<DataDictionary> datas)
        {
            return IDDM.Edits(datas);
        }
        public static List<FDataDictionary> fdataDictionaries;
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
            return IDDM.LIST(Argument);
        }

        /// <summary>
        /// 根据 父级Argument、子级枚举值  获取子级对象
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="value">子级枚举值</param>
        /// <returns></returns>
        public DataDictionary ITEM(string Argument, int value)
        {
            return IDDM.ITEM(Argument, value);
        }
        /// <summary>
        /// 根据父级Argument，子级名称，获取子级对象
        /// </summary>
        /// <param name="Argument"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataDictionary ITEM(string Argument, string name)
        {
            return IDDM.ITEM(Argument, name);
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
            return IDDM.VALUE(Argument, key);
        }
        /// <summary>
        ///  根据父级Argument、子级名称 获取自己枚举值（仅允许g_config使用）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int VALUE_CONFIG(string name)
        {
            return IDDM.VALUE_CONFIG(name);
        }
        /// <summary>
        /// 获取成员 姓 的拼音
        /// </summary>
        /// <param name="surname">姓</param>
        /// <returns></returns>
        public string GetUserNamePY(string surname)
        {
            return IDDM.GetUserNamePY(surname);
        }

        /// <summary>
        /// 根据 父级Argument、子级名称  获取子级枚Custom
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="key">子级名称</param>
        /// <returns></returns>
        public string GetCustom(string Argument, string key)
        {
            return IDDM.GetCustom(Argument, key);
        }
        /// <summary>
        /// 根据 父级Argument、子级枚举值  获取子级枚Custom
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="value">子级名称</param>
        /// <returns></returns>
        public string GetCustom(string Argument, int val)
        {
            return IDDM.GetCustom(Argument, val);
        }
        #endregion

        /// <summary>
        /// 网站启动的时候初始化字典
        /// </summary>
        public void InitList()
        {
            IDDM.InitList();
        }
    }
}
