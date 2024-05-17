using BusinessLogic.Repository;
using System.Data;
using System.Reflection;
using System.Text;



namespace BusinessLogic.Concrete
{
    public class CommonFunctionConcrete: ICommonFunction
    {
        static int GetRepeatedChars(string str, char strChar)
        {
            int count = 0;
            if (str.Length > 0)
            {
                for (int j = 0; j < str.Length; j++)
                {
                    if (strChar == str[j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public bool CheckFileFormat(IFormFile file)
        {
            int c = 0;
            if (file != null && GetRepeatedChars(file.FileName, '.') == 1)
            {
                string filename = "";
                filename = file.FileName.ToLower();
                if (!filename.Contains(".php"))
                {
                    string ext = System.IO.Path.GetExtension(filename).ToLower();
                    c = 1;
                }
                else
                {
                    c = 0;
                }
                if (c == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (!(dr[column.ColumnName] is DBNull))
                            pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
        public string CreatePassword(int length)
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz@#$";
            System.Text.StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public bool CheckImageFormat(IFormFile file)
        {
            int c = 0;
            if (file != null)
            {
                string filename = "";
                filename = file.FileName.ToLower();
                if (!filename.Contains(".php"))
                {
                    string ext = System.IO.Path.GetExtension(filename).ToLower();
                    if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                    {
                        c = 1;
                    }
                    else
                    {
                        c = 0;
                    }
                }
                else
                {
                    c = 0;
                }
                if (c == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
