using System;
using System.IO;
using System.Text;
using Xunit;

namespace UnitTestProject461
{
    public class GenerateDbScriptByTemplate
    {
        [Fact]
        public void GenerateDbScriptByTemplate()
        {
            string filename = "Insert_CodesCustomer_DbScriptByTemplate";

            //string fileToSearch = @"C:\Users\fred_sena\DataFile.txt";
           
            var builder = new StringBuilder();
            //builder.Append(GetDatafromFile(fileToSearch));

            string path = @"C:\2021-0602\";

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string[] row = GetInputData().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < row.Length; i++)
            {
                if (string.IsNullOrEmpty(row[i].Trim())) continue;

                string[] data = row[i].Split(new[] {";"}, StringSplitOptions.None);

                builder.AppendLine();

                builder.Append(GetTemplate()
                    .Replace("#Code#", data[0].Trim())
                    .Replace("#Customer#", data[1].Trim())
                    .Replace("#number#", (i+1).ToString()));

                builder.AppendLine();
            }

            string filePath = path + "\\" + filename + ".txt";
            File.WriteAllText(filePath, builder.ToString().Substring(0, builder.Length));
            builder = null;
        }

        public string GetTemplate()
        {
            return @"
-- #number#.  #Code# #Customer#
IF (NOT EXISTS (
SELECT COUNT(test) qtd
FROM Code PCCS
WHERE PCCS.CustomerId in (Select id from Customer where code = '#Customer#')
GROUP BY PCCS.CustomerId ))

BEGIN
BEGIN TRY
INSERT INTO [dbo].[CodeCustomer] ([CodeId] ,[CustomerId])
SELECT (select id from Code where Code = '#Code#') as CodeId, (Select id from Customer where code = '#Customer#') as CustomerId

PRINT 'SUCCESS: Customer code: #Customer# for Code: #Code# was successfully INSERTED on the CodeCustomer table'
END TRY
BEGIN CATCH
PRINT 'FAILED: Customer code: #Customer# for Code: #Code# was NOT INSERTED on CodeCustomer table due to: ' + ERROR_MESSAGE()
END CATCH
END
ELSE
BEGIN
IF (EXISTS ( SELECT COUNT(test) qtd
FROM CodeCustomer PCCS
WHERE PCCS.CustomerId in (Select id from Customer where code = '#Customer#')
GROUP BY PCCS.CustomerId
HAVING COUNT(PCCS.CustomerId) = 1 ))

BEGIN
BEGIN TRY
UPDATE PaymentCodeCustomer SET PaymentCodeId = (SELECT id from code where code = '#Code#')
WHERE CustomerId = (SELECT id from Customer where Code = '#Customer#')

PRINT 'SUCCESS: Customer code: #Customer# for Code: #Code# was successfully updated on the CodeCustomer table'
END TRY
BEGIN CATCH
PRINT 'FAILED: Customer code: #Customer# for Code: #Code# was NOT updated on CodeCustomer table due to: ' + ERROR_MESSAGE()
END CATCH
END
ELSE
PRINT 'FAILED: Customer code: #Customer# for Code: #Code# was was NOT updated on the CodeCustomer table'
END";
        }


        public string GetInputData()
        {
            return @"data1;value1
data2;value2
data3;value3";

        }
        public string GetDatafromFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath, Encoding.GetEncoding("ISO-8859-1"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }

}
