using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SQLite;
using System.Collections.Generic;

namespace RecoveriesConnect
{
	public class DAL
	{
		public static bool insertInboxItem(Inbox data, string path)
		{
			try
			{
				var db = new SQLiteConnection(path);
				var id = db.Insert(data);
				if ( id == 0)
				{
					return false;
				}
				else
					return true;
				
			}
			catch (SQLiteException ex)
			{
				return false;
			}
		}

		public static bool updateInboxItem(Inbox data, string path)
		{
			try
			{
				var db = new SQLiteConnection(path);
				string[] temp = new string[5];
				temp[0] = data.MessagePathText;
				temp[1] = data.Status;
				temp[2] = data.IsLocal;
				temp[3] = data.FileName1;
				temp[4] = data.MessageNo;


				var isUpdated = db.Execute("UPDATE Inbox set MessagePathText=?, Status =?, IsLocal=?, FileName1= ? Where MessageNo=?", temp);
				if (isUpdated == 0)
				{
					return false;
				}
				else
					return true;

			}
			catch (SQLiteException ex)
			{
				return false;
			}
		}

		public static bool deleteInboxItem(Inbox data, string path)
		{
			try
			{
				var db = new SQLiteConnection(path);

				string[] temp = new string[1];

				temp[0] = data.MessageNo;

				var isDeleted = db.Execute("DELETE FROM Inbox Where MessageNo=?", temp);
				if (isDeleted == 0)
				{
					return false;
				}
				else
					return true;

			}
			catch (SQLiteException ex)
			{
				return false;
			}
		}


		public static void DeleteAll(string path) {
			try
			{
				var db = new SQLiteConnection(path);

				db.DeleteAll<Inbox>();
			}
			catch (SQLiteException ex)
			{
				
			}
		}

		public static List<Inbox> GetAll(string path)
		{
			var db = new SQLiteConnection(path);

			return db.Query<Inbox>("Select * from Inbox");
		}

		public static List<Inbox> GetByMessageNo(string path, string MessageNo)
		{
			var db = new SQLiteConnection(path);
			return db.Query<Inbox>("Select * from Inbox where MessageNo = ?", MessageNo);
		}

		public static int findNumberRecords(string path)
		{
			try
			{
				var db = new SQLiteConnection(path);
				// this counts all records in the database, it can be slow depending on the size of the database
				var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Inbox");

				// for a non-parameterless query
				// var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person WHERE FirstName="Amy");

				return count;
			}
			catch (SQLiteException)
			{
				return -1;
			}
		}

	}
}

