Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

function EndRequestHandler(sender, args) {
    
    if (args.get_error() != undefined && args.get_error().httpStatusCode == '500') {

        var errorMessage = args.get_error().message.replace(args.get_error().name + ': ', '(#_#) CHÚ Ý -> LỖI HỆ THỐNG:<br/><br/>')
        args.set_errorHandled(false);

        Alert_Message(errorMessage);
    }
}