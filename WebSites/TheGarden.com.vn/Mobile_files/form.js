if (typeof action == 'undefined') {
    var action = { version:"1.0.0",
        date:"03-11-2011",
        author:"Ngannv",
        msg:'',
        actionKey:'',
        actResult:'',
        actionReturn:'',
        ajaxUrl:WEB_DIR + 'ajax.php',
        conf:function ()
        {
        }, runAjax:function (act, code, data, callBack, cache)
        {
            var data = data;
            var ajax_url = this.ajaxUrl + '?act=' + act + '&code=' + code;
            this.actionKey = ((ajax_url + JSON.stringify(data)));
            if (cache && eCache.hasItem(this.actionKey)) {
                var f = eCache.getItem(this.actionKey);
                return eval(callBack(f));
            }
            jQuery.ajax({ type:'POST', url:ajax_url, data:data, dataType:'json', success:function (jsonRes)
            {
                if (cache) {
                    eCache.setItem(action.actionKey, jsonRes);
                }
                return eval(callBack(jsonRes));
            } });
        } };
    window.action = action;
    action.conf();
}
if (typeof frm == 'undefined') {
    var frm = { version:"1.0.0", date:"03-11-2011", author:"Ngannv", templates:new Array(), curFormKey:'', formData:new Array(),
        cacheForm:false,
        formId:'popup_join',
        width:'', top:20, getOldHtml:false, isUploadForm:false, formActionUrl:'',
        isForm:true,
        focusId:'',
        isDraggable:true, join:function (str)
        {
            var store = [str];
            return function extend(other)
            {
                if (other != null && 'string' == typeof other) {
                    store.push(other);
                    return extend;
                }
                return store.join('');
            }
        }, _enable_keyboard_action:function ()
        {
            jQuery(document).keydown(function (objEvent)
            {
                if (objEvent == null) {
                    keycode = event.keyCode;
                    escapeKey = 27;
                }
                else {
                    keycode = objEvent.keyCode;
                    escapeKey = objEvent.DOM_VK_ESCAPE;
                }
                var key = String.fromCharCode(keycode).toLowerCase();
                if (( keycode == escapeKey ) || ( keycode == 27 )) {
                    frm.close('callBack');
                }
            });

        }, close:function (isCallBack)
        {
            if (frm.isForm) {
                frm.formData[frm.curFormKey] = jQuery('#' + frm.formId).serializeArray();
            }
            else {
                frm.isForm = true;
            }
            jQuery.unblockUI();
            if (isCallBack != undefined && action.actionReturn != '') {
                var act = action.actionReturn;
                action.actionReturn = '';
                return eval(act);
            }
        }, showFormAjax:function (act, code, data, isForm, cache)
        {
            if (isForm != undefined) {
                frm.isForm = isForm;
            }
            else {
                frm.isForm = true;
            }
            var data = data;
            var ajax_url = WEB_DIR + 'ajax.php?act=' + act + '&code=' + code + '';
            frm.curFormKey = ajax_url.trim() + data;
            if ((cache != undefined && cache == true) && eCache.hasItem(frm.curFormKey)) {
                var f = eCache.getItem(frm.curFormKey);
                frm.show(f);
                if (frm.formData[frm.curFormKey] != undefined) {
                    var formData = frm.formData[frm.curFormKey];
                    for (var i in formData) {
                        if (jQuery('#' + frm.formId + ' #' + formData[i]['name']) != null) {
                            jQuery('#' + frm.formId + ' #' + formData[i]['name']).attr('value', formData[i]['value']);
                        }
                    }
                }
                return false;
            }
            jQuery.ajax({ type:'POST', url:ajax_url, data:data, dataType:'json', success:function (json)
            {
                if (json.template) {
                    eCache.setItem(frm.curFormKey, json.template);
                    frm.show(json.template);
                }
                else {
                    alert(json.msg);
                }
            } });
            return false;
        }};
    window.frm = frm;
    frm._enable_keyboard_action();
}

frm.show = function (data)
{
    var top = eval(jQuery(window).scrollTop() + parseInt(frm.top));
    frm.top = 0;
    var ext = '';
    if (frm.isUploadForm == true) {
        ext += ' method="post" enctype="multipart/form-data" ';
    }
    if (frm.formActionUrl != '') {
        ext += ' action="' + frm.formActionUrl + '" ';
    }
    var startForm = '<div id="' + frm.formId + '">';
    var endForm = '</div>';
    if (frm.isForm == true) {
        startForm = '<form name="' + frm.formId + '" id="' + frm.formId + '"' + ext + '>';
        endForm = '</form>';
    }
    jQuery.blockUI({message:jQuery(frm.join(startForm + data + endForm)()), overlayCSS:{ opacity:0.3, background:'#000000', height:'100%', cursor:'default'}, centerY:false, css:{ top:top + 'px', border:'none', width:'100%', margin:'0 auto', 'background-color':'transparent',position:'absolute'}});
    if (jQuery('#singlePopup').css('position') == 'fixed') {
        eLibs.getScreenSize();
        var dialogTop = Math.floor((eLibs.screenHeight / 3) - (jQuery('#singlePopup').height() / 2));
        var dialogLeft = Math.floor((eLibs.screenWidth / 2) - (jQuery('#singlePopup').width() / 2));
        jQuery('#singlePopup').css({top:dialogTop + 'px', left:dialogLeft + 'px'});
    }
    jQuery("#popup_join").draggable({ revert:false });
    if (frm.focusId != '') {
        jQuery(frm.focusId).focus();
        frm.focusId = '';
    }
};
frm.addOption = function (el, text, value, selected, color)
{
    var optn = document.createElement("option");
    optn.text = text;
    optn.value = value;
    if (selected != undefined) {
        optn.selected = selected;
    }
    if (selected != undefined) {
        optn.style.color = color;
    }

    el.options.add(optn);

};
frm.switchSelectMultiOption = function (select_tag_id_from, select_tag_id_to)
{
    var userSelect = new Array();
    jQuery('#' + select_tag_id_to + " option").each(function (i)
    {
        userSelect[i] = jQuery(this).val();
    });
    var objSelect = document.getElementById(select_tag_id_to);
    jQuery('#' + select_tag_id_from + " option:selected").each(function ()
    {
        var id = jQuery(this).val();
        if (id != -1) {
            if (userSelect.length > 0) {
                if (jQuery.inArray(id, userSelect) < 0) frm.addOption(objSelect, jQuery(this).text(), jQuery(this).val(), true);
            }
            else {
                frm.addOption(objSelect, jQuery(this).text(), jQuery(this).val(), true);
            }
            jQuery(this).remove();
        }
    });
};
frm.reset = function (all_data, conf)
{
    if (conf == undefined) {
        if (!confirm("Dữ liệu của bạn có thể không khôi phục lại được\nBạn có chắc muốn thực hiện hành động này không?")) {
            return false;
        }
    }
    jQuery(':input', '#' + frm.formId).not(':button, :submit, :reset, :hidden').val('').removeAttr('checked').removeAttr('selected');
    if (all_data != undefined && all_data != false) {
        frm.formData = new Array();
    }
};
frm.saveCurFormData = function (doOtherAction, feedBackOtherAction)
{
    frm.formData[frm.curFormKey] = jQuery('#' + frm.formId).serializeArray();
    if (feedBackOtherAction != undefined) {
        action.actionReturn = feedBackOtherAction;
    }
    if (doOtherAction != undefined) {
        return eval(doOtherAction);
    }
};
