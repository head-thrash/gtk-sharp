using Gnome.Vfs;
using System;
using System.Text;

namespace Test.Gnome.Vfs {
	public class TestSyncCreate {
		static void Main (string[] args)
		{
			if (args.Length != 1) {
				Console.WriteLine ("Usage: TestSyncCreate <uri>");
				return;
			}
		
			Gnome.Vfs.Vfs.Initialize ();

			Gnome.Vfs.Uri uri = new Gnome.Vfs.Uri (args[0]);
			
			FilePermissions perms = FilePermissions.UserRead |
						FilePermissions.UserWrite |
						FilePermissions.GroupRead |
						FilePermissions.OtherRead;
			Console.WriteLine (perms);
			Handle handle = Sync.Create (uri, OpenMode.Write, false, perms);

			UTF8Encoding utf8 = new UTF8Encoding ();
			Result result = Result.Ok;
			Console.WriteLine ("Enter text and end with Ctrl-D");
			while (result == Result.Ok) {
				string line = Console.ReadLine ();
				if (line == null)
					break;
				byte[] buffer = utf8.GetBytes (line);

				ulong bytesWritten;
				result = Sync.Write (handle, out buffer[0],
						     (ulong)buffer.Length, out bytesWritten);
				Console.WriteLine ("result write '{0}' = {1}", uri, result);
				Console.WriteLine ("{0} bytes written", bytesWritten);
			}
			
			result = Sync.Close (handle);
			Console.WriteLine ("result close '{0}' = {1}", uri, result);
			
			Gnome.Vfs.Vfs.Shutdown ();
		}
	}
}
