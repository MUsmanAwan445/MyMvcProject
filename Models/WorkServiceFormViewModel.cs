

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; 
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcProject.Models
{
    public class WorkServiceFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Reference Number is required.")]
        [StringLength(100)]
        public string? RefNo { get; set; }

        [Required(ErrorMessage = "Recorded By is required.")]
        [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "Numbers are not allowed. Please use letters only.")]
        public string? RecordedBy { get; set; }

        [Required(ErrorMessage = "Officer Incharge is required.")]
        [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "Numbers are not allowed. Please use letters only.")]
        public string? OfficerIncharge { get; set; }

        [Required(ErrorMessage = "Name Tasked is required.")]
        [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "Numbers are not allowed. Please use letters only.")]
        public string? NameTasked { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string? Designation { get; set; }

        [Required(ErrorMessage = "Estimated Cost is required.")]
        public decimal? EstimatedCost { get; set; }

        [Required(ErrorMessage = "Details attached at is required.")]
        public string? DetailsAttachedAt { get; set; }

        [Required(ErrorMessage = "Verified By Officer is required.")]
        [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "Numbers are not allowed. Please use letters only.")]
        public string? VerifiedByOfficer { get; set; }

        [Required(ErrorMessage = "Please select Store Status.")]
        public string? StoreStatus { get; set; }

        [Required(ErrorMessage = "Recommendation is required.")]
        public string? Recommendation { get; set; }

        [Required(ErrorMessage = "Please select a Sanction Type.")]
        public string? SanctionType { get; set; }

        [Required(ErrorMessage = "Technical Sanction Dir(W&S) is required.")]
        [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "Numbers are not allowed. Please use letters only.")]
        public string? TechnicalSanctionDir { get; set; }

        [Required(ErrorMessage = "Execution Officer Incharge is required.")]
        [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "Numbers are not allowed. Please use letters only.")]
        public string? ExecutionOfficerIncharge { get; set; }

        [Required(ErrorMessage = "Execution Officer Designation is required.")]
        public string? ExecutionOfficerDesignation { get; set; }

        [Required(ErrorMessage = "Deployed Staff name is required.")]
        [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "Numbers are not allowed. Please use letters only.")]
        public string? DeployedStaff { get; set; }

        [Required(ErrorMessage = "Deployed Staff Designation is required.")]
        public string? DeployedStaffDesignation { get; set; }

        [Required(ErrorMessage = "Recoupment Dir(W&S) is required.")]
        [RegularExpression(@"^[a-zA-Z\s\.]+$", ErrorMessage = "Numbers are not allowed. Please use letters only.")]
        public string? RecoupmentDirWS { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        // --- 🔥 PROFILE PICTURE PROPERTIES ADDED HERE 🔥 ---
        
        // Database mein image ka path save karne ke liye
         public string? ProfilePicPath { get; set; }

        [NotMapped]
        public IFormFile? ProfileImage { get; set; }
    }
}