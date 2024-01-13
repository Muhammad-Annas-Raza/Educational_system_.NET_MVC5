# Educational_system_.NET_MVC5
<div><b>First Project</b> 02-Sep-2022</div>
<div><strong>" eStudiez "</strong><div>
<div>An educational System using .Net MVC 5 (Code First Approach)</div>
<div>In this system eStudiez is a fictional name of a educational system</div>

========================================== important notes ======================================================
<br>git config http.postBuffer 524288000              //When push is not done through git terminal
<br>https://www.guru99.com/deploying-website-iis.html //To setup IIS in server machine

<br>https://kandi.openweaver.com/?landingpage=python_all_codesnippets&utm_source=youtube&utm_medium=cpc&utm_campaign=promo_kandi_ie&utm_content=kandi_ie_buildwith&utm_term=python_devs&gclid=Cj0KCQjwgLOiBhC7ARIsAIeetVCB_EKLkVhwoUxJtFA0Ccepj2VVCseD6_eAeORizIJHO4qAW5MgcVIaAplAEALw_wcB


<br>Scaffold-DbContext “server=DESKTOP-83AVVG7\MSSQLSERVER19;database=iSOL_POS;User ID=sa;Password=Super@123;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
//Database First Approach in .Net 7.0 Scaffold Command

<br>When Models are not being made from scaffolding (Database First Approach) in MVC5 
https://stackoverflow.com/questions/76388322/cannot-import-anymore-tables-into-edmx-running-transformation-system-nullrefe

<br>
Scaffold-DbContext 'Name=ConnectionStrings:cs' Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force

<br>
https://www.youtube.com/watch?v=moqe9mh3j2c&feature=youtu.be
<br>
Setup IIS on Windows 10

<br><br><br>
Reseed Query to over come SQL SERVER abnormal behaviour
<br>
DBCC CHECKIDENT ('table_name', RESEED, new_value); 
 
<br><br><br>
3 - Layered Architecture Demo 
<br>
https://enlabsoftware.com/development/how-to-build-and-deploy-a-three-layer-architecture-application-with-c-sharp-net-in-practice.html
<br>
Github Repo : https://github.com/EnLabSoftware/Three-layer_architecture_sample
<br><br>

<br>
Convert Base64 in to byte (image) through C#
<br><br><br>
<br><br><br>
<br><br><br>

public ActionResult ImageReport()
        {
            try
            {
                string Token = "Lg==|ZGJfVHJpcEVycA==|c2E=|c3Fs";
                CS = $"data source = {Decrypt(Token.Split('|')[0])}; initial catalog = {Decrypt(Token.Split('|')[1])}; user id = {Decrypt(Token.Split('|')[2])}; password = {Decrypt(Token.Split('|')[3])}";
                SqlConnection con = null;
                DataTable ds = new DataTable();
                try
                {
                    using (con = new SqlConnection(CS))
                    {
                        string query = "select Img_idd,Img_Form,Img_Name,dbo.Base64ToVarBinary(Img_Image) as Image from MP_Images where RIGHT(Img_Name, 3) = 'png'";
                        SqlDataAdapter sda = new SqlDataAdapter(query, con);
                        sda.Fill(ds);

                        // Add a new column named "Image" with byte[] data type
                        ds.Columns.Add("Image", typeof(byte[]));

                        // Populate the "Image" column with byte array data
                        foreach (DataRow row in ds.Rows)
                        {
                            // Assuming the column "Img_Image" contains your Base64 strings
                            string base64String = row["Img_Image"].ToString();
                            byte[] byteArray = Convert.FromBase64String(base64String);
                            row["Image"] = byteArray;
                        }

                        GenerateReport(ds, "Reports", "MP_Image", "ImageDocument", "");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.ErrorMsg = e.Message;
                }
                return View();


            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

        }

<br>
Convert Base64 in to byte (image) through Sql Server Custom Function
<br><br><br>
-- Function <Start>--<br><br><br>

IF OBJECT_ID('dbo.Base64ToVarBinary', 'FN') IS NULL
BEGIN
    EXEC('
        CREATE FUNCTION dbo.Base64ToVarBinary (@base64String NVARCHAR(MAX))
        RETURNS VARBINARY(MAX)
        AS
        BEGIN
            DECLARE @result VARBINARY(MAX);
            SET @result = CAST('''' AS XML).value(''xs:base64Binary(sql:variable("@base64String"))'', ''VARBINARY(MAX)'');
            RETURN @result;
        END;
    ');
END;
<br><br><br>
-- Function <End>--









