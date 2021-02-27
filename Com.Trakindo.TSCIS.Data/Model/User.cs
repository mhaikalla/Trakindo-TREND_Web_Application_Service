using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("UserTsics")]
    public class User
    {
        [Key]

        public int UserId {get; set;}
        public string Name {get; set;}
        public string Username	{get; set;}
        public int EmployeeId	{get; set;}
        public DateTime? Dob {get; set;}
        public string Position	{get; set;}
        public string Email	{get; set;}
        public string Phone { get; set;}
        public string AreaCode { get; set;}
        public string AreaName { get; set;}
        public string BranchCode { get; set;}
        public string BranchName { get; set;}
        public int RoleId { get; set;}
        public DateTime? CreatedAt { get; set;}
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }
        public string MobilePassword { get; set;}
        public string MobileToken { get; set;}
        public int Approval1 { get; set; }
        public int Approval2 { get; set; }
        public int IsAdmin { get; set; }
        public int MasterAreaId { get; set; }
        public int MasterBranchId { get; set; }
        public string RoleDescription { get; set; }
        public string NameApproval1 { get; set; }
        public DateTime? DateApproval1 { get; set;}
        public string NameApproval2 { get; set; }
        public DateTime? DateApproval2 { get; set;}
        public int UserTsManager1Id { get; set; }
        public string UserTsManager1Name { get; set; }
        public DateTime? UserTsManager1DueDate { get; set; }
        public int UserTsManager2Id { get; set; }
        public string UserTsManager2Name { get; set; }
        public DateTime? UserTsManager2DueDate { get; set; }
        public int UserTsManager3Id { get; set; }
        public string UserTsManager3Name { get; set; }
        public DateTime? UserTsManager3DueDate { get; set; }
        public int UserTsManagerDueFlag { get; set; }
        public DateTime? DueDateApproval1 { get; set; }
        public DateTime? DueDateApproval2 { get; set; }
        public string TechnicalJobExperienceDuration { get; set; }
        public string TechnicalJobTitle { get; set; }
        public string PhotoProfile { get; set; }
        public string AdminPassword { get; set; }
        public int IsDelegate { get; set; }
        public string POH_Name { get; set; }
        public string PlayerId { get; set; }
    }

    public class UserAPI
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? Dob { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }
        public string MobilePassword { get; set; }
        public string MobileToken { get; set; }
        public int Approval1 { get; set; }
        public int Approval2 { get; set; }
        public int IsAdmin { get; set; }
        public int MasterAreaId { get; set; }
        public int MasterBranchId { get; set; }
        public string RoleDescription { get; set; }
        public string NameApproval1 { get; set; }
        public DateTime? DateApproval1 { get; set; }
        public string NameApproval2 { get; set; }
        public DateTime? DateApproval2 { get; set; }
        public int UserTsManager1Id { get; set; }
        public string UserTsManager1Name { get; set; }
        public DateTime? UserTsManager1DueDate { get; set; }
        public int UserTsManager2Id { get; set; }
        public string UserTsManager2Name { get; set; }
        public DateTime? UserTsManager2DueDate { get; set; }
        public int UserTsManager3Id { get; set; }
        public string UserTsManager3Name { get; set; }
        public DateTime? UserTsManager3DueDate { get; set; }
        public int UserTsManagerDueFlag { get; set; }
        public DateTime? DueDateApproval1 { get; set; }
        public DateTime? DueDateApproval2 { get; set; }
        public string TechnicalJobExperienceDuration { get; set; }
        public string TechnicalJobTitle { get; set; }
        public string PhotoProfile { get; set; }
        public string AdminPassword { get; set; }
        public int IsDelegate { get; set; }
        public string POH_Name { get; set; }
        public string PlayerId { get; set; }
    }
}
