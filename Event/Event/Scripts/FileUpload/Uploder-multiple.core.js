
var DataURLFileReader = {
    read: function (file, callback) {
        var reader = new FileReader();
        var fileInfo = {
            name: file.name,
            type: file.type,
            fileContent: null,
            size: function () {
                var FileSize = 0;
                if (file.size > 1048576) {
                    FileSize = Math.round(file.size * 100 / 1048576) / 100 + " MB";
                }
                else if (file.size > 1024) {
                    FileSize = Math.round(file.size * 100 / 1024) / 100 + " KB";
                }
                else {
                    FileSize = file.size + " bytes";
                }
                return file.size;
            }
        };
        callback(CheckFileType(fileInfo.name), fileInfo);
        return;
        //if (FileSize = Math.round(file.size * 100 / 1048576) / 100 > 20) {
        //    callback("Tổng dung lượng các file không được quá 20 MB", fileInfo);
        //    return;
        //}
        //else
        //{
        //    callback(CheckFileType(fileInfo.name), fileInfo);
        //    return; 
        //}

        reader.onload = function () {
            fileInfo.fileContent = reader.result;
            callback(null, fileInfo);
        };
        reader.onerror = function () {
            callback(reader.error, fileInfo);
        };
        reader.readAsDataURL(file);
    }
};

function CheckFileType(fname)
{
    var validateFile = ["pdf", "doc", "docx", "xls", "xlsx", "rar", "zip", "jpg", "jpeg", "png","txt","ppt", "pptx", "bmp"];
    var extention = fname.slice((Math.max(0, fname.lastIndexOf(".")) || Infinity) + 1);
    var lowerCase = extention.toLowerCase();
    if (validateFile.indexOf(lowerCase) < 0)
    {
        if(lang == "en")
            return "Only support file fomat " + validateFile.join(", ");
        else
            return "Chương trình chỉ hỗ trợ các file định dạng " + validateFile.join(", ");
    }        
    else
        return "";
}

function MultiplefileSelected(evt) {
    evt.stopPropagation();
    evt.preventDefault();

    timeSelectFile++;

    selectedFiles = evt.target.files || evt.dataTransfer.files;
    if (selectedFiles) {
        
        for (var i = 0; i < (selectedFiles.length) ; i++) {
            DataURLFileReader.read(selectedFiles[i], function (err, fileInfo) {
                var FileSize = 0;
                if (fileInfo.size() > 1048576) {
                    FileSize = Math.round(fileInfo.size() * 100 / 1048576) / 100 + " MB";
                }
                else if (fileInfo.size() > 1024) {
                    FileSize = Math.round(fileInfo.size() * 100 / 1024) / 100 + " KB";
                }
                else {
                    FileSize = fileInfo.size() + " bytes";
                }
                fileSizeTotal += fileInfo.size();
                if (Math.round(fileSizeTotal * 100 / 1048576) / 100 > 20)
                {                   
                    if (lang == "en")
                        $("#errorFileSize").html("The total size of all files should not exceed 20MB");
                    else
                        $("#errorFileSize").html("Tổng dung lượng các file vượt quá 20 MB");
                }
                var fname = fileInfo.name;
                if (err != null) {
                    var RowInfo = '<tr id="File_'  +timeSelectFile.toString()+ i.toString() + '">' +
                                  '<td style="font-size:11px">' + fileInfo.name + '<br /> <font color="red">' + err + '</font> </td>' +
                                  '<td style="font-size:11px; width:80px; text-align: center; vertical-align:middle">' + FileSize + '</td>' +
                                  '<td style="width:50px; text-align: center; vertical-align:middle"><button onclick="RemoveFile(' + timeSelectFile.toString() + i.toString() + ')" type="button" class="btn btn-default btn-xs" aria-hidden="true"><i class="fa fa-remove"></i></button></td></tr>';
                    $('#Files').append(RowInfo);
                    if (err == "")
                    {
                        var SelectedFileObject = { time: timeSelectFile.toString() + i.toString(), file: selectedFiles[i] }
                        selectedFileTimes.push(SelectedFileObject);
                    }
                }
                else {
                    var RowInfo = '<tr id="File_' + i + '" class="info">' +
                                  '<td style="font-size:11px">' + fileInfo.name + '</td>' +
                                  '<td style="font-size:11px; width:80px; text-align: center; vertical-align:middle">' + FileSize + '</td>' +
                                  '<td style="width:50px; text-align: center; vertical-align:middle"><button type="button" class="btn btn-default btn-xs" aria-hidden="true"><i class="fa fa-remove"></i></button></td></tr>';
                    $('#Files').append(RowInfo);
                    selectedFileTimes.push(selectedFiles[i]);
                }
            });
        }
    }
}

$(function () {
    $.ajaxSetup({ cache: false });

    $(".create_modalUpload").on("click", function (e) {
        

        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#CRUDDialogContent').html(AlertDialog());

        $('#CRUDDialog').modal({
            backdrop: 'static',
            keyboard: true
        }, 'show');

        //$('#CRUDDialog').addClass("modal-wide");

        $('#CRUDDialogContent').load(this.href, function (response, status, xhr) {
            if (status == "error") {
                $("#CRUDDialogContent").html(AlertErrorDialog());
            }
            else {              
                bindFormUpload(this);
                var height = $(window).height() - 130;
                var elm = $(this).find(".modal-body");
                $(this).find(".modal-body").css("max-height", height);
            }
            //bindFormUpload(this);
            //var height = $(window).height() - 130;
            //var elm = $(this).find(".modal-body");
            //$(this).find(".modal-body").css("max-height", height);
        });
        return false;
    });
});

function bindFormUpload(dialog) {

    $('form', dialog).submit(function () {

        if (Math.round(fileSizeTotal * 100 / 1048576) / 100 > 20) {
            return false;
        }

        //$('.submit').button('loading');
        var dataString = new FormData(this);
        
        //for (var i = 0; i < userArray.length; i++) {
        //    dataString.append("userPermision", { Id: userArray[i].userObj.Id, FullName: userArray[i].userObj.FullName, Dept: userArray[i].userObj.Dept, Email: userArray[i].userObj.Email, Notify: userArray[i].userObj.Notify });
        //}
        for (var i = 0; i < selectedFileTimes.length; i++) {
            dataString.append("uploadedFiles", selectedFileTimes[i].file);
        }       
        if (fileRemoveId.length > 0)
        {
            for (var i = 0; i < fileRemoveId.length; i++)
            dataString.append("fileRemoveId", fileRemoveId[i]);
        }
        
        $("#ProgresingFile").show();
        $.ajax({
            url: this.action,
            type: this.method,
            traditional: true,
            xhr: function () {
                var myXhr = $.ajaxSettings.xhr();
                if (myXhr.upload) { // Check if upload property exists
                    myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // For handling the progress of the upload
                }
                return myXhr;
            },
            data: dataString,            
            success: function (result) {
                if (result.statusCode == 200) {
                    $('#CRUDDialog').modal('hide');
                    formSubmit(1);
                } else {
                    $('#callback').html(result);
                    $('#CRUDDialogContent').find(".modal-body").scrollTop(0);
                    $('#CRUDDialogContent').find(".submit").button('reset');
                    $("#ProgresingFile").hide();

                    //$('#CRUDDialogContent').html(result);
                    //var height = $(window).height() - 180;
                    //var elm = $(this).find(".modal-body");
                    //$('#CRUDDialogContent').find(".modal-body").css("max-height", height);
                    //bindFormUpload();
                }
            },
            error: function (xhr, status, error) {
                $('#CRUDDialogContent').html(AlertErrorDialog());
            },
            contentType: false,
            processData: false
        });
       
        return false;
    });
}

function Init_Multiple_Upload() {
    $("#UploadedFiles").change(function (evt) {
        MultiplefileSelected(evt);
    });    
    //$('#CreateDialog button[id=Submit_btn]').click(function () {
    //    UploadMultipleFiles();
    //});       
}

function RemoveFile(fileId) {
   
    $('#File_' + fileId).remove();

    var obj = { time: fileId, file: null }
    for (var i = 0; i < selectedFileTimes.length; i++)
    {
        var timeId = selectedFileTimes[i].time;
        if (timeId == fileId) {
            fileSizeTotal -= selectedFileTimes[i].file.size;
            selectedFileTimes.splice(i, 1);            
            break;
        }
    }
    if (Math.round(fileSizeTotal * 100 / 1048576) / 100 <= 20) {
        $("#errorFileSize").html("");
    }
    return false;
}

function CountSelectedFile()
{
    alert(selectedFileTimes.length);
}

