using System.Collections.Generic;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Role
    {
        public Role()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; }



        public bool Add_User { get; set; }
        public bool Edit_User { get; set; }
        public bool Remove_User { get; set; }
        public bool View_User { get; set; }
        public bool Validation_User { get; set; }


        public bool Add_Role { get; set; }
        public bool Edit_Role { get; set; }
        public bool Remove_Role { get; set; }
        public bool View_Role { get; set; }


        public bool Add_Category { get; set; }
        public bool Edit_Category { get; set; }
        public bool Remove_Category { get; set; }
        public bool View_Category { get; set; }


        public bool Add_Item { get; set; }
        public bool Edit_Item { get; set; }
        public bool Remove_Item { get; set; }
        public bool View_Item { get; set; }


        public bool Add_Attribute { get; set; }
        public bool Edit_Attribute { get; set; }
        public bool Remove_Attribute { get; set; }
        public bool View_Attribute { get; set; }


        public bool Add_Unit { get; set; }
        public bool Edit_Unit { get; set; }
        public bool Remove_Unit { get; set; }
        public bool View_Unit { get; set; }


        public bool Add_InsTitute { get; set; }
        public bool Edit_InsTitute { get; set; }
        public bool Remove_InsTitute { get; set; }
        public bool View_InsTitute { get; set; }


        public bool Add_Yeareducation { get; set; }
        public bool Edit_Yeareducation { get; set; }
        public bool Remove_Yeareducation { get; set; }
        public bool View_Yeareducation { get; set; }

        public bool Add_Grade { get; set; }
        public bool Edit_Grade { get; set; }
        public bool Remove_Grade { get; set; }
        public bool View_Grade { get; set; }

        public bool Add_Class { get; set; }
        public bool Edit_Class { get; set; }
        public bool Remove_Class { get; set; }
        public bool View_Class { get; set; }

        public bool Add_Course { get; set; }
        public bool Edit_Course { get; set; }
        public bool Remove_Course { get; set; }
        public bool View_Course { get; set; }

        public bool Add_OrgChart { get; set; }
        public bool Edit_OrgChart { get; set; }
        public bool Remove_OrgChart { get; set; }
        public bool View_OrgChart { get; set; }

        public bool Add_OrgPerson { get; set; }
        public bool Edit_OrgPerson { get; set; }
        public bool Remove_OrgPerson { get; set; }
        public bool View_OrgPerson { get; set; }

        public bool Add_Education { get; set; }
        public bool Edit_Education { get; set; }
        public bool Remove_Education { get; set; }
        public bool View_Education { get; set; }

        public bool Add_Insurance { get; set; }
        public bool Edit_Insurance { get; set; }
        public bool Remove_Insurance { get; set; }
        public bool View_Insurance { get; set; }

        public bool Add_Salary { get; set; }
        public bool Edit_Salary { get; set; }
        public bool Remove_Salary { get; set; }
        public bool View_Salary { get; set; }

        public bool Add_Teacher { get; set; }
        public bool Edit_Teacher { get; set; }
        public bool Remove_Teacher { get; set; }
        public bool View_Teacher { get; set; }

        public bool Edit_TeacherPassword { get; set; }
        public bool Edit_TeacherAllAccess { get; set; }

        public bool Add_TimeandDays { get; set; }
        public bool Edit_TimeandDays { get; set; }
        public bool Remove_TimeandDays { get; set; }
        public bool View_TimeandDays { get; set; }

        public bool Add_TimeSchedule { get; set; }
        public bool Edit_TimeSchedule { get; set; }
        public bool Remove_TimeSchedule { get; set; }
        public bool View_TimeSchedule { get; set; }

        public bool Add_Student { get; set; }
        public bool Edit_Student { get; set; }
        public bool Remove_Student { get; set; }
        public bool View_Student { get; set; }

        public bool View_StudentStudyRecord { get; set; }
        public bool View_StudentWorkbook { get; set; }

        public bool View_StudentFinancialWidget { get; set; }

        public bool Edit_StudentPassword { get; set; }
        public bool Edit_StudentParentPassword { get; set; }


        public bool Add_Exam { get; set; }
        public bool Edit_Exam { get; set; }
        public bool Remove_Exam { get; set; }
        public bool View_Exam { get; set; }

        public bool View_ExamAnalize { get; set; }

        public bool Add_ExamType { get; set; }
        public bool Edit_ExamType { get; set; }
        public bool Remove_ExamType { get; set; }
        public bool View_ExamType { get; set; }

        public bool Add_ExamScore { get; set; }
        public bool Edit_ExamScore { get; set; }
        public bool Remove_ExamScore { get; set; }
        public bool View_ExamScore { get; set; }

        public bool Add_Question { get; set; }
        public bool Edit_Question { get; set; }
        public bool Remove_Question { get; set; }
        public bool View_Question { get; set; }

        public bool Add_QuestionOption { get; set; }
        public bool Edit_QuestionOption { get; set; }
        public bool Remove_QuestionOption { get; set; }
        public bool View_QuestionOption { get; set; }

        public bool Add_PaymentType { get; set; }
        public bool Edit_PaymentType { get; set; }
        public bool Remove_PaymentType { get; set; }
        public bool View_PaymentType { get; set; }

        public bool Add_StdPayment { get; set; }
        public bool Edit_StdPayment { get; set; }
        public bool Remove_StdPayment { get; set; }
        public bool View_StdPayment { get; set; }

        public bool Add_Contract { get; set; }
        public bool Edit_Contract { get; set; }
        public bool Remove_Contract { get; set; }
        public bool View_Contract { get; set; }

        public bool Add_ContractType { get; set; }
        public bool Edit_ContractType { get; set; }
        public bool Remove_ContractType { get; set; }
        public bool View_ContractType { get; set; }



        // INDEX -------------------------------------------------


        public bool Add_Post { get; set; }
        public bool Edit_Post { get; set; }
        public bool Remove_Post { get; set; }
        public bool View_Post { get; set; }

        // POSTTYPE ------------------------------------------------


        public bool post_feed { get; set; }
        public bool post_post { get; set; }
        public bool post_fadak { get; set; }
        public bool post_amoozesh { get; set; }
        public bool post_enzebati { get; set; }
        public bool post_parvaresh { get; set; }
        public bool post_mali { get; set; }
        public bool post_it { get; set; }
        public bool post_moshaver { get; set; }
        public bool post_voroodBeSystem { get; set; }
        public bool post_sabteNam { get; set; }
        public bool post_mokatebat { get; set; }
        public bool post_ghesmathayeSamane { get; set; }
        public bool post_faq { get; set; }
        public bool post_ehrazeHoviat { get; set; }
        public bool post_sharayetSabteNam { get; set; }
        public bool post_darkhastTajdidNazar { get; set; }
        public bool post_enteghadVaPishnahad { get; set; }
        public bool post_daneshAmoozan { get; set; }
        public bool post_daneshAmookhtegan { get; set; }
        public bool post_forsatHayeShoghli { get; set; }
        public bool post_tamasBaTaha { get; set; }
        public bool post_darbareTaha { get; set; }
        public bool post_dabirKhaneBargozidegan { get; set; }
        public bool post_hedayatTahsil { get; set; }


        // /POSTTYPE ------------------------------------------------

        public bool Add_Comment { get; set; }
        public bool Edit_Comment { get; set; }
        public bool Remove_Comment { get; set; }
        public bool View_Comment { get; set; }

        public bool Add_MainSlideShow { get; set; }
        public bool Edit_MainSlideShow { get; set; }
        public bool Remove_MainSlideShow { get; set; }
        public bool View_MainSlideShow { get; set; }

        public bool Add_Schedule { get; set; }
        public bool Edit_Schedule { get; set; }
        public bool Remove_Schedule { get; set; }
        public bool View_Schedule { get; set; }

        public bool Add_Advertising { get; set; }
        public bool Edit_Advertising { get; set; }
        public bool Remove_Advertising { get; set; }
        public bool View_Advertising { get; set; }

        public bool Add_PictureGallery { get; set; }
        public bool Edit_PictureGallery { get; set; }
        public bool Remove_PictureGallery { get; set; }
        public bool View_PictureGallery { get; set; }

        public bool Add_Picture { get; set; }
        public bool Edit_Picture { get; set; }
        public bool Remove_Picture { get; set; }
        public bool View_Picture { get; set; }

        public bool Add_BestStudent { get; set; }
        public bool Edit_BestStudent { get; set; }
        public bool Remove_BestStudent { get; set; }
        public bool View_BestStudent { get; set; }


        // /INDEX -------------------------------------------------



        public bool Add_StudentType { get; set; }
        public bool Edit_StudentType { get; set; }
        public bool Remove_StudentType { get; set; }
        public bool View_StudentType { get; set; }


        public bool Add_Notification { get; set; }
        public bool Edit_Notification { get; set; }
        public bool Remove_Notification { get; set; }
        public bool View_Notification { get; set; }




        public bool Add_ClassBook { get; set; }
        public bool Edit_ClassBook { get; set; }
        public bool Remove_ClassBook { get; set; }
        public bool View_ClassBook { get; set; }



        public bool Add_Writer { get; set; }
        public bool Edit_Writer { get; set; }
        public bool Remove_Writer { get; set; }
        public bool View_Writer { get; set; }

        public bool Add_ProductCategory { get; set; }
        public bool Edit_ProductCategory { get; set; }
        public bool Remove_ProductCategory { get; set; }
        public bool View_ProductCategory { get; set; }

        public bool Add_Product { get; set; }
        public bool Edit_Product { get; set; }
        public bool Remove_Product { get; set; }
        public bool View_Product { get; set; }

        public bool Add_Link { get; set; }
        public bool Edit_Link { get; set; }
        public bool Remove_Link { get; set; }
        public bool View_Link { get; set; }


        public bool Add_ScoreThemplate { get; set; }
        public bool Edit_ScoreThemplate { get; set; }
        public bool Remove_ScoreThemplate { get; set; }
        public bool View_ScoreThemplate { get; set; }

        public bool Add_StudentScore { get; set; }
        public bool Edit_StudentScore { get; set; }
        public bool Remove_StudentScore { get; set; }
        public bool View_StudentScore { get; set; }

        

        public bool Add_VirtualTeaching { get; set; }
        public bool Edit_VirtualTeaching { get; set; }
        public bool Remove_VirtualTeaching { get; set; }
        public bool View_VirtualTeaching { get; set; }



        public bool Add_Workbook { get; set; }
        public bool Edit_Workbook { get; set; }
        public bool Remove_Workbook { get; set; }
        public bool View_Workbook { get; set; }

        public bool Add_OnlineExam { get; set; }
        public bool Edit_OnlineExam { get; set; }
        public bool Remove_OnlineExam { get; set; }
        public bool View_OnlineExam { get; set; }

        public bool Add_OnlineExamOption { get; set; }
        public bool Edit_OnlineExamOption { get; set; }
        public bool Remove_OnlineExamOption { get; set; }
        public bool View_OnlineExamOption { get; set; }

        public bool Add_OnlineExamResult { get; set; }
        public bool Edit_OnlineExamResult { get; set; }
        public bool Remove_OnlineExamResult { get; set; }
        public bool View_OnlineExamResult { get; set; }


        public bool Add_OnlineClass { get; set; }
        public bool Edit_OnlineClass { get; set; }
        public bool Remove_OnlineClass { get; set; }
        public bool View_OnlineClass { get; set; }


        public bool Add_OnlineClassServer { get; set; }
        public bool Edit_OnlineClassServer { get; set; }
        public bool Remove_OnlineClassServer { get; set; }
        public bool View_OnlineClassServer { get; set; }


        public bool Add_DescriptiveScore { get; set; }
        public bool Edit_DescriptiveScore { get; set; }
        public bool Remove_DescriptiveScore { get; set; }
        public bool View_DescriptiveScore { get; set; }


        public bool View_StudentDailySchedule { get; set; }


        public bool View_Log { get; set; }


        public virtual IList<User> Users { get; set; }


        public bool haveAnyUser
        {
            get
            {
                if (Users == null)
                {
                    return false;
                }
                return Users.Any();
            }
        }
    }
}
