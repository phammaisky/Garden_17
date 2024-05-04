/**
 * Created with JetBrains PhpStorm.
 * User: ngannv
 * Date: 10/29/12
 * Time: 1:52 AM
 * To change this template use File | Settings | File Templates.
 */
if (typeof TGD == 'undefined') {
    var TGD = { ajaxAct:'project' };
    window.TGD = TGD;
}
TGD.showShopIntro = function (shopId, storeName, cateid) {
    var code = 'getShopIntro';
    var act = 'project';
    var data = 'shopId=' + shopId + '&cateId=' + cateid;
    var callBack = function (json) {
        if (json != undefined) {
            if (json.msg == 'success') {

                jQuery('#shopIntroView2').hide();
                jQuery('#shopIntroView').html(json.template);
                jQuery('#storeName').html(storeName);
                /*   jQuery('.scroll-pane').jScrollPane();*/
                return false;
            } else if (json.msg != 'not-found') {
                alert(json.msg);
                return false;
            } else if (json.msg == 'not-found') {
                jQuery('#shopIntroView').html(json.notfound);
            }
        } else {
            alert('Thao tác không thành công. Vui lòng thử lại');
        }
    };
    return action.runAjax(act, code, data, callBack, true);
};
TGD.loadCateByStore = function (storeId) {
    var code = 'loadCateByStore';
    var act = 'project';
    var data = 'storeId=' + storeId + '';
    var callBack = function (json) {
        if (json != undefined) {
            if (json.msg == 'success') {
                jQuery('#shopIntroView').html(json.template);
                return false;
            } else if (json.msg != 'not-found') {
                alert(json.msg);
                return false;
            }
        } else {
            alert('Thao tác không thành công. Vui lòng thử lại');
        }
    };
    return action.runAjax(act, code, data, callBack, true);
};
TGD.checkTickRootCate = function (rootId) {
    var flag = false;
    jQuery('input.checkCate_' + rootId).each(function () {
        var check = jQuery(this).attr('checked');
        if (check == true || check == 'checked') {
            flag = true;
        }

    });
    if (flag == true) {
        jQuery('#cate_' + rootId).attr('checked', 'checked');
    }
};

TGD.showContactAboutUs = function () {
    var code = 'showContactAboutUs';
    var act = 'project';
    var data = null;
    var callBack = function (json) {
        if (json != undefined) {
            if (json.msg == 'success') {
                frm.show(json.template);
                return false;
            } else if (json.msg != '') {
                alert(json.msg);
                return false;
            }
        } else {
            alert('Thao tác không thành công. Vui lòng thử lại');
        }
    };
    return action.runAjax(act, code, data, callBack, false);
};
TGD.showLeaseForm = function () {
    var code = 'showLeaseForm';
    var act = 'project';
    var data = null;
    var callBack = function (json) {
        if (json != undefined) {
            if (json.msg == 'success') {
                frm.show(json.template);
                return false;
            } else if (json.msg != '') {
                alert(json.msg);
                return false;
            }
        } else {
            alert('Thao tác không thành công. Vui lòng thử lại');
        }
    };
    return action.runAjax(act, code, data, callBack, false);
};
TGD.showRegGift = function (event_id) {
    var code = 'showRegGift';
    var act = 'project';
    var data = 'event_id='+event_id;
    var callBack = function (json) {
        if (json != undefined) {
            if (json.msg == 'success') {
                frm.show(json.template);
                return false;
            } else if (json.msg != '') {
                alert(json.msg);
                return false;
            }
        } else {
            alert('Thao tác không thành công. Vui lòng thử lại');
        }
    };
    return action.runAjax(act, code, data, callBack, false);
};
TGD.showNewsLetter = function () {
    var code = 'showNewsLetter';
    var act = 'project';
    var data = null;
    var callBack = function (json) {
        if (json != undefined) {
            if (json.msg == 'success') {
                frm.show(json.template);
                return false;
            } else if (json.msg != '' && json.msg != null) {
                alert(json.msg);
                return false;
            }
        } else {
            alert('Thao tác không thành công. Vui lòng thử lại');
        }
    };
    return action.runAjax(act, code, data, callBack, false);
};

TGD.regNewLetter = function () {
	if (jQuery('#accept_news').is(':checked'))
	{

		var data = jQuery('#' + frm.formId + '').serializeArray();
		var code = 'regNewLetter';
		var callBack = function (json)
		{
			if (json.flag == 0) {
				alert('Bạn vui lòng nhập đầy đủ thông tin\nPlease input full info.');
				return false;
			}
			else if (json.flag == '1') {
				alert("Cảm ơn bạn đã đăng ký nhận bản tin!\nThanks for register newsletter!");
				frm.close();
			}
			return false
		};

		action.runAjax(this.ajaxAct, code, data, callBack, true);
	}
	else{
		alert('Bạn không thể thực hiện thao tác này nếu không tích chọn chấp nhận nhận thông tin!\nPlease click and accept the Acceptment Checkbox!');
	}
};
TGD.sendFeedBack = function () {

    var data = jQuery('#' + frm.formId + '').serializeArray();
    var code = 'sendFeedBack';
    var callBack = function (json) {
        if (json.flag == 0) {
            alert('Bạn vui lòng nhập đầy đủ thông tin\nPlease input full info.');
            return false;
        }
        else if (json.flag == '1') {
            alert('Thanks for send feedback!');
            location.reload();
            //return Staff.showAddJobAndAvatarForm(json.staff_id)
        }
        return false
    };
    action.runAjax(this.ajaxAct, code, data, callBack, true);
};
TGD.sendRegGift = function () {

    var data = jQuery('#' + frm.formId + '').serializeArray();
    var code = 'sendRegGift';
    var callBack = function (json) {
        if (json.flag == 0) {
            alert(json.msg);
            return false;
        }
        else if (json.flag == '1') {
            alert('Thanks for send feedback!');
            location.reload();
            //return Staff.showAddJobAndAvatarForm(json.staff_id)
        }
        return false
    };
    action.runAjax(this.ajaxAct, code, data, callBack, true);
};
TGD.configIdNewsPromotion = function (store_id, cate_id) {
    var code = 'configIdNewsPromotion';
    var data = 'store_id=' + store_id + '&cate_id=' + cate_id + '&news_id=' + jQuery('#newsIdPromotionConfig').val();
    var callBack = function (json) {
        if (json.msg == 'ok') {
            alert('Cấu hình tin khuyến mại cho gian hàng và chuyên mục thành công');
            return false;
        } else {
            alert(json.msg);
        }
    };
    action.runAjax(this.ajaxAct, code, data, callBack, false);
};
TGD.fillDataToTextboxBySelectOption = function (selectOptionId, fillTo) {

};

eLibs.goTopStart();