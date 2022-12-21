using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CIS2103_Website.Models
{
    public class DVDs : ControllerBase
    {
        Database db = new Database();
        public IActionResult AddDVDCode(IFormCollection fc)
        {

            string addDVDQuery;
            if (fc["DVDImage"] != "NULL")
            {
                IFormFile file = fc.Files.GetFile("DVDImage")!;
                byte[] image = null;
                if (file.Length > 0)

                //Convert Image to byte and save to database

                {


                    using (var fs1 = file.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        image = ms1.ToArray();
                    }

                }
                Console.WriteLine(BitConverter.ToString(image));

                /*addDVDQuery = "INSERT INTO DVD VALUES('" + fc["DVDName"] + "',CONVERT(VARBINARY(max),'" + BitConverter.ToString(image) + "'),'" + fc["Quantity"]
                                 + "','" + fc["Description"] + "','" + fc["Category"] + "','" + fc["RatePerRent"] + "')";*/
                addDVDQuery = "INSERT INTO DVD (dvdName, dvdImage, quantity, description, category, ratePerRent) VALUES('" + fc["DVDName"] + "'," + image + ",'" + fc["Quantity"]
                                 + "','" + fc["Description"] + "','" + fc["Category"] + "','" + fc["RatePerRent"] + "')";
            }
            else
            {
                addDVDQuery = "INSERT INTO DVD (dvdName, quantity, description, category, ratePerRent)" +
                              "VALUES('" + fc["DVDName"] + "','" + fc["Quantity"] + "','" + fc["Description"] +
                              "','" + fc["Category"] + "','" + fc["RatePerRent"] + "')";
            }

            db.Query(addDVDQuery);
            return Ok("DVD Added Sucessfully");
        }

        public IActionResult EditDVDCode(IFormCollection fc)
        {

            string editDVDQuery;
            if (fc["DVDImage"] != "NULL")
            {
                IFormFile file = fc.Files.GetFile("DVDImage")!;
                byte[] image = null;
                if (file.Length > 0)

                //Convert Image to byte and save to database

                {


                    using (var fs1 = file.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        image = ms1.ToArray();
                    }

                }
                Console.WriteLine(BitConverter.ToString(image));

                /*addDVDQuery = "INSERT INTO DVD VALUES('" + fc["DVDName"] + "',CONVERT(VARBINARY(max),'" + BitConverter.ToString(image) + "'),'" + fc["Quantity"]
                                 + "','" + fc["Description"] + "','" + fc["Category"] + "','" + fc["RatePerRent"] + "')";*/
                editDVDQuery = "UPDATE DVD SET dvdName='" + fc["DVDName"] + "',dvdImage='" + image + "',quantity='" + fc["Quantity"]
                                 + "',description='" + fc["Description"] + "',category='" + fc["Category"] + "',ratePerRent='" + fc["RatePerRent"] + "'  WHERE dvdId='" + fc["DVDId"] + "'";
            }
            else
            {
                editDVDQuery = "UPDATE DVD SET dvdName='" + fc["DVDName"] + "',quantity='" + fc["Quantity"]
                                 + "',description='" + fc["Description"] + "',category='" + fc["Category"] + "',ratePerRent='" + fc["RatePerRent"] + "' WHERE dvdId='" + fc["DVDId"] + "'";
            }

            db.Query(editDVDQuery);
            return Ok("DVD Updated Sucessfully");
        }

        public IActionResult GetAllDVDsCode()
        {
            string getDvdIdsQuery = "SELECT dvdId FROM DVD";
            List<string> dvdIds = db.ReadMultipleDataQuery(getDvdIdsQuery, "dvdId");
            int dvdIdCount = dvdIds.Count;
            DVDModel[] dvds = new DVDModel[dvdIdCount];

            for (int i = 0; i < dvdIdCount; i++)
            {
                string getDvdQuery = "SELECT * FROM DVD WHERE dvdId=" + dvdIds[i];
                dvds[i] = db.ReadDVDQuery(getDvdQuery);
            }
            return Ok(dvds);
        }

    }

    public class DVDModel
    {
        public int? DVDId { get; set; }
        public string? DVDName { get; set; }
        public byte[]? DVDImage { get; set; }
        public int? Quantity { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal? RatePerRent { get; set; }
    }
}
