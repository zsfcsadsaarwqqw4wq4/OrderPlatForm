using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IManager
{
    public interface IDataDictionaryManager
    {
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="entity">参数实体对象</param>
        void Add(DataDictionaryAddParams entity);
        /// <summary>
        /// 删除
        /// </summary>
        void Delete(DeleteDataDictionaryParams @params);
        /// <summary>
        /// 删除子级
        /// </summary>
        /// <param name="fid">父级ID</param>
        /// <param name="id">子级ID</param>
        void DeleteC(int fid, int id);
        /// <summary>
        /// 删除父级
        /// </summary>
        /// <param name="id">父级ID</param>
        void DeleteF(int id);
        /// <summary>
        /// 编辑字典
        /// </summary>
        /// <param name="entity">参数实体对象</param>
        void Edit(EdtiDataDictionary entity);
        /// <summary>
        /// 编辑父级字典
        /// </summary>
        /// <param name="changedDataDictionary">父级实体对象</param>
        void Edit(FDataDictionary changedDataDictionary);
        /// <summary>
        /// 编辑子级字典
        /// </summary>
        /// <param name="changedDataDictionary">子级实体对象</param>
        void Edit(DataDictionary changedDataDictionary);
        /// <summary>
        /// 查询字典数据
        /// </summary>
        /// <returns></returns>
        object Query();
        /// <summary>
        /// 根据名称查询字典数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object Query(string name);
        object Query1();
        /// <summary>
        /// 根据枚举值获取子级字典
        /// </summary>
        /// <param name="fID">父级ID</param>
        /// <param name="cID">子级ID</param>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        DataDictionary GetDataDictionary(int fID, int value);
        /// <summary>
        /// 根据子级id查询子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataDictionary GetChilderDataDetail(int id);
        /// <summary>
        /// 根据子级id查询子级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<DataDictionary> GetChilderDataList(string ids);
        /// <summary>
        /// 批量修改 改custom
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        bool Edits(List<DataDictionary> datas);

        /// <summary>
        /// 根据父级Argument  获取子级列表
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <returns></returns>
        List<DataDictionary> LIST(string Argument);
        /// <summary>
        /// 根据 父级Argument、子级枚举值  获取子级对象
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="value">子级枚举值</param>
        /// <returns></returns>
        DataDictionary ITEM(string Argument, int value);

        /// <summary>
        /// 根据父级Argument，子级名称，获取子级对象
        /// </summary>
        /// <param name="Argument"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        DataDictionary ITEM(string Argument, string name);
        /// <summary>
        /// 根据 父级Argument、子级名称、子级枚举值  判断子级枚举值是否正确
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="key">子级名称</param>
        /// <param name="value">子级枚举值</param>
        /// <returns></returns>
        bool CHECK(string Argument, string key, int value);

        /// <summary>
        /// 根据 父级Argument、子级名称  获取子级枚举值
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="key">子级名称</param>
        /// <returns></returns>
        int VALUE(string Argument, string key);

        /// <summary>
        ///  根据父级Argument、子级名称 获取自己枚举值（仅允许g_config使用）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int VALUE_CONFIG(string name);

        /// <summary>
        /// 获取成员 姓 的拼音
        /// </summary>
        /// <param name="surname">姓</param>
        /// <returns></returns>
        string GetUserNamePY(string surname);
        /// <summary>
        /// 根据 父级Argument、子级名称  获取子级枚Custom
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="key">子级名称</param>
        /// <returns></returns>
        string GetCustom(string Argument, string key);
        /// <summary>
        /// 根据 父级Argument、子级枚举值  获取子级枚Custom
        /// </summary>
        /// <param name="Argument">父级Argument</param>
        /// <param name="value">子级名称</param>
        /// <returns></returns>
        string GetCustom(string Argument, int val);
        /// <summary>
        /// 网站启动的时候初始化字典
        /// </summary>
        void InitList();
    }
}
