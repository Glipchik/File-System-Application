# Introduction 
It is a console application for working with files. Using it, you can copy, move, rename and delete some files. 
Also this application stores some information about your files in the form of metadata.
---

# Getting Started
First of all, you can change login and password in "appsettings.json" file. Secondly, program will work with files, including 
saving files to certain directory, and you need to clarify what this directory will be. 
To do it you can go to "appsettings.json" file and change StoragePath value.
Also to start the application you need to be logged in. To do it you have to write `--l bluewhale777`(or the login you 
changed to the existing one). Then write `--p yngallways`(or the password you changed to the existing one). 
Metainformation about files is storing in the database, so you need to add connection string to the database in "appsettings.json".
---

# Working with files
There are some commands using which you can work with files:
- `file upload "path-to-file"` (for example, file upload "~/movies/sci-fi/k-pax.mkv") - download file from **path-to-file** to the storage
- `file download "file-name" "destination-path"` - download **file-name** from the storage to **destination-path**(it is not necessary to enter the full path, it can be just name of the folder)
- `file move "source-file-name" "destination-file-name"` - rename file in the storage from **source-file-name** to **destination-file-name**
- `file remove "file-name"` - delete **file-name** from the storage
- `file info "file-name"` - get information about **file-name**
Also you can get information about current account:
- `user info`
And you can export information about storing files as xml or json file:
- `file export "destination-path" --format <format>` (for example, file export "~/work/meta-info.json" --format json). If the format isn't clarified 
json will be used.
- `file export --info` - shows what formats are supported
And you can exit from program with 'exit' command
---

# Requirements
For the correct operation of the application you will need at least 1.2 gigabytes of hard storage memory.
---

# Restrictions
Maximum upload file size is 50 megabytes. Maximum storage size is 1 gigabyte.
---