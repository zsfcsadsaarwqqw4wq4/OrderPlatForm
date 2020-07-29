using Common;
using Domain;
using IManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Domain.ExModel;

namespace Manager
{
    public class ClassFicationManager: IClassFicationManager,IDependency
    {
        /// <summary>
        /// 返回结果泛型类
        /// </summary>
        public ResultPageData<object> pagedata = new ResultPageData<object>();
        public ResultPageData<object> QueryClass(int pageIndex, int pageSize)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.ClassiFication
                             where a.PID==0 && a.Shape==1
                             select new
                             {
                                 fdata=new
                                 {
                                     ID = a.ID,
                                     ClassName = a.ClassName,
                                     RecordTime = a.RecordTime,
                                     Status = a.Status,
                                     PID = a.PID
                                 },
                                 data = (from b in db.ClassiFication
                                        where b.PID==a.ID && a.Shape == 1
                                         select new
                                        {
                                            ID = b.ID,
                                            ClassName = b.ClassName,
                                            RecordTime = b.RecordTime,
                                            Status = b.Status,
                                            PID = b.PID
                                        })
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = db.Set<ClassiFication>().Count(o=>o.PID==0 && o.Shape == 1);
                return pagedata;
            }
        }
        public ResultPageData<object> QueryClass(int pageIndex, int pageSize,string ClassName)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.ClassiFication
                             where a.PID == 0 && a.ClassName.Equals(ClassName) && a.Shape == 1
                             select new
                             {
                                 fdata = new
                                 {
                                     ID = a.ID,
                                     ClassName = a.ClassName,
                                     RecordTime = a.RecordTime,
                                     Status = a.Status,
                                     PID = a.PID
                                 },
                                 data = (from b in db.ClassiFication
                                         where b.PID == a.ID && a.Shape == 1
                                         select new
                                         {
                                             ID = b.ID,
                                             ClassName = b.ClassName,
                                             RecordTime = b.RecordTime,
                                             Status = b.Status,
                                             PID = b.PID
                                         })
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = db.Set<ClassiFication>().Count(o => o.PID == 0 && o.Shape == 1);
                return pagedata;
            }
        }
        public ResultPageData<object> QueryClass(int pageIndex, int pageSize, int ID)
        {
            using (ShopEntities db = new ShopEntities())
            {
                var Qurey = (from a in db.ClassiFication
                             where a.ID==ID && a.Shape == 1
                             select new
                             {
                                 fdata = new
                                 {
                                     ID = a.ID,
                                     ClassName = a.ClassName,
                                     RecordTime = a.RecordTime,
                                     Status = a.Status,
                                     PID = a.PID
                                 },
                                 data = (from b in db.ClassiFication
                                         where b.PID == a.ID && a.Shape == 1
                                         select new
                                         {
                                             ID = b.ID,
                                             ClassName = b.ClassName,
                                             RecordTime = b.RecordTime,
                                             Status = b.Status,
                                             PID = b.PID
                                         })
                             }).ToList();
                pagedata.data = Qurey.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagedata.total = db.Set<ClassiFication>().Count(o => o.PID == 0 && o.Shape == 1);
                return pagedata;
            }
        }
        /// <summary>
        /// 获取全部分类信息接口
        /// </summary>
        public List<ClassiFication> QueryClassFication()
        {
            using (ShopEntities db = new ShopEntities())
            {
                return db.ClassiFication.ToList();
            }
        }
        /// <summary>
        /// 添加分类对象
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        public bool AddClassiFication(ClassiFication cf)
        {
            using (ShopEntities db = new ShopEntities())
            {
                db.ClassiFication.Add(cf);
                return db.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// 编辑分类对象
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        public bool EditClassiFication(ClassiFication cf)
        {
            using (ShopEntities db = new ShopEntities())
            {
                ClassiFication cfs = db.ClassiFication.SingleOrDefault(u => u.ID == cf.ID);
                if (cf.Status == null)
                {
                    cfs.ClassName = cf.ClassName;
                    return db.SaveChanges() > 0;
                }
                else
                {
                    if (cf.ClassName!=null)
                    {
                        cfs.Status = cf.Status;
                        cfs.ClassName = cf.ClassName;
                        return db.SaveChanges() > 0;
                    }
                    else
                    {
                        cfs.Status = cf.Status;
                        return db.SaveChanges() > 0;
                    }
                }
            }
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="cf"></param>
        /// <returns></returns>
        public bool RemoveClassiFication(ClassiFication cf)
        {
            using (ShopEntities db = new ShopEntities())
            {
                bool flag = false;
                List<ClassiFication> list=db.ClassiFication.Where(o => o.PID == cf.ID).ToList();
                if (list.Count != 0)
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Set<ClassiFication>().Attach(cf);
                            db.Entry<ClassiFication>(cf).Property("Shape").IsModified = true;
                            //关闭实体有效验证
                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();
                            foreach (var item in list)
                            {
                                item.Shape = 0;
                                db.Set<ClassiFication>().Attach(item);
                                db.Entry<ClassiFication>(item).Property("Shape").IsModified = true;
                                db.SaveChanges();
                            }
                            transaction.Commit();
                            //恢复验证实体有效性（ValidateOnSaveEnabled）这个开关【如果后续有其他操作，记得恢复】
                            db.Configuration.ValidateOnSaveEnabled = true;
                            flag = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                        return flag;

                    }                  
                }
                else
                {

                    db.Set<ClassiFication>().Attach(cf);
                    db.Entry<ClassiFication>(cf).Property("Shape").IsModified = true;
                    //关闭实体有效验证
                    db.Configuration.ValidateOnSaveEnabled = false;
                    if (db.SaveChanges() > 0)
                    {
                        //恢复验证实体有效性（ValidateOnSaveEnabled）这个开关【如果后续有其他操作，记得恢复】
                        db.Configuration.ValidateOnSaveEnabled = true;
                        flag = true;
                        return flag;
                    }
                    else
                    {
                        //恢复验证实体有效性（ValidateOnSaveEnabled）这个开关【如果后续有其他操作，记得恢复】
                        db.Configuration.ValidateOnSaveEnabled = true;
                        return flag;
                    }
                }       
            }
        }
        /// <summary>
        /// 根据分类名查询是否有父级  0表示没有查找到数据，-1表示查找到的数据没有父级 ，其他数字表示查找的数据有父级
        /// </summary>
        public int QueryClassIsParent(string ClassName)
        {
            int flag = 0;
            using (ShopEntities db = new ShopEntities())
            {
                var res=db.ClassiFication.SingleOrDefault(o=>o.ClassName.Equals(ClassName));
                if (res == null)
                {
                    return flag;
                }
                else
                {
                    if (res.PID == 0)
                    {
                        flag = -1;
                        return flag;
                    }
                    else
                    {
                        flag = Convert.ToInt32(res.PID);
                        return flag;
                    }
                }
            }
        }
        
    }
}
