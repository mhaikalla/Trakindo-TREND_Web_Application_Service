using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Web.Http;
using TSICS.Helper;
using Com.Trakindo.TSICS.Data.Context;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.MobileApi
{

    public class MobileLoginController : ApiController
    {
        private readonly UserRoleBusinessService _UserRoleBusinessService = Factory.Create<UserRoleBusinessService>("UserRole", ClassType.clsTypeBusinessService);
        // GET: api/MobileLogin
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET: api/MobileLogin/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MobileLogin
        public IHttpActionResult Post(MobileLogin plainTextLoginData)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("delete from UserDevices where PlayerId = '' or PlayerId = null");
            }
            //Check User Login
            UserBusinessService userBs = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);

            MobileLogin loginData = new MobileLogin
            {
                Xupj = plainTextLoginData.Xupj, Password = Common.Base64Encode(plainTextLoginData.Password)
            };
            User userM = userBs.CheckLoginMobile(loginData);
            
            if (userM != null)
            {
                var usrd = new UserDevices {UserId = userM.UserId, PlayerId = plainTextLoginData.PlayerId};
                userM.MobileToken = Common.AccessToken() + userM.UserId;
                userM.PlayerId = plainTextLoginData.PlayerId;
                userBs.Edit(userM);
                
                if (usrd.PlayerId != null)
                {
                    userBs.InsertData(usrd);
                    
                }
                if (!string.IsNullOrWhiteSpace(userM.PhotoProfile))
                {
                    userM.PhotoProfile = WebConfigure.GetDomain() + "/Upload/UserProfile/" + userM.PhotoProfile;
                }
                else
                {
                    userM.PhotoProfile = WebConfigure.GetDomain() + "/assets/images/repository/avatar-default.jpg";
                }
              
                UserAPI userData = new UserAPI()
                {
                    UserId = userM.UserId,
                    Name = userM.Name,
                    EmployeeId = userM.EmployeeId,
                    AreaName = string.IsNullOrWhiteSpace(userM.AreaName) ? "HEAD OFFICE" : userM.AreaName,
                    BranchName = string.IsNullOrWhiteSpace(userM.BranchName) ? "" : userM.BranchName,
                    CreatedAt = userM.CreatedAt,
                    AreaCode = userM.AreaCode,
                    BranchCode = userM.BranchCode,
                    MasterAreaId = userM.MasterAreaId,
                    MasterBranchId = userM.MasterBranchId,
                    Email = userM.Email,
                    RoleId = userM.RoleId,
                    RoleDescription = userM.RoleDescription,
                    RoleName = userM.RoleId ==0 || _UserRoleBusinessService.GetDetail(userM.RoleId) == null ? "GUEST" : _UserRoleBusinessService.GetDetail(userM.RoleId).Name,
                    Dob = userM.Dob,
                    IsDelegate = userM.IsDelegate,
                    MobileToken = userM.MobileToken,
                    PhotoProfile = userM.PhotoProfile,
                    POH_Name = userM.POH_Name,
                    Status = userM.Status,
                    MobilePassword = userM.MobilePassword,
                    Position = userM.Position,
                    Phone = userM.Phone,
                    PlayerId = userM.PlayerId,
                    Username = userM.Username,
                    TechnicalJobExperienceDuration = userM.TechnicalJobExperienceDuration,
                    TechnicalJobTitle = userM.TechnicalJobTitle,
                };
                Delegation userD = userBs.GetDetailDelegationByUserFrom(userM.UserId);
                if (userD != null)
                {
                    DelegationAPI delegationAPI = new DelegationAPI()
                    {
                        DelegateId = userD.DelegateId,
                        StartDate = userD.StartDate.Value.ToString("dddd, dd MMMM yyyy"),
                        EndDate = userD.EndDate.Value.ToString("dddd, dd MMMM yyyy"),
                        ToUser = userBs.GetDetail(userD.ToUser),
                        Status = userD.Status,
                        CreatedAt = userD.CreatedAt.Value.ToString("dddd, dd MMMM yyyy")

                    };
                    return Ok(new { Code = 200, Authorization = "true", Data = userData, delegation = delegationAPI });
                }
                else
                {
                    DelegationAPI delegationAPI = new DelegationAPI()
                    {
                        DelegateId =0,
                        StartDate =null ,
                        EndDate =null ,
                        FromUser = null,
                        ToUser = null,
                        Status = 0,
                        CreatedAt = null
                    };
                    return Ok(new { Code = 200, Authorization = "true", Data = userData, delegation = delegationAPI });
                }
            }
            else
            {
                return Ok(new { Code = 401, Authorization = "false", Message = "Gagal Login" });
            }
        }

        // PUT: api/MobileLogin/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MobileLogin/5
        public void Delete(int id)
        {
        }
    }
}
