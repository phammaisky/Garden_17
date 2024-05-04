using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMS.Models;

namespace AMS.Repositories
{
    public class DeviceAndToolReposity
    {
        public static void CreateDeviceAndTool(DeviceToolAndHistory deviceToolAndHistory, int userId)
        {
            using (var db = new AMSEntities())
            {
                using (var dbcxtransaction = db.Database.BeginTransaction())
                {
                    var deviceAndTool = new DeviceAndTool();
                    deviceAndTool.CompanyId = deviceToolAndHistory.CompanyId;
                    deviceAndTool.DeviceCatId = deviceToolAndHistory.DeviceCatId;
                    deviceAndTool.ToolCatId = deviceToolAndHistory.ToolCatId;
                    if (deviceToolAndHistory.AutoAssetsCode)
                        deviceAndTool.AssetsCode = AMS.Controllers.FunctionsGeneral.generateDeviceAndToolCode();
                    else
                        deviceAndTool.AssetsCode = deviceToolAndHistory.AssetsCode;

                    deviceAndTool.DeviceName = deviceToolAndHistory.DeviceName;
                    deviceAndTool.DescriptionDevice = deviceToolAndHistory.DescriptionDevice;
                    deviceAndTool.BuyDate = deviceToolAndHistory.BuyDate;

                    deviceAndTool.CreateDate = DateTime.Now;
                    deviceAndTool.CreateById = userId;

                    db.DeviceAndTools.Add(deviceAndTool);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting
                                // the current instance as InnerException
                                raise = new InvalidOperationException(message, raise);
                            }
                        }                        
                    }
                   

                    var historyUse = new HistoryUse();
                    historyUse.DeviceToolId = deviceAndTool.Id;
                    historyUse.HandedToStaffId = deviceToolAndHistory.StaffId;
                    historyUse.HandedDate = deviceToolAndHistory.HandedDate;
                    historyUse.DeptId = deviceToolAndHistory.DeptId;
                    historyUse.LocationId = deviceToolAndHistory.LocationId;
                    historyUse.StatusId = deviceToolAndHistory.StatusId;
                    historyUse.StatusDescrition = deviceToolAndHistory.StatusDescription;
                    db.HistoryUses.Add(historyUse);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting
                                // the current instance as InnerException
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                    }
                   
                    dbcxtransaction.Commit();
                }
            }
        }

    }
}