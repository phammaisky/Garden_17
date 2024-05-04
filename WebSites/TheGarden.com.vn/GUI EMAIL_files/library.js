/*
#####################
# Code by AzNet team
# AzNet library
# hoangnova
#####################
*/
jQuery(document).ready(function(){
	//begin
	jQuery().ajaxSend(function(r,s){
		jQuery("#loading-layer").show();
	});

	jQuery().ajaxStop(function(r,s){
		jQuery("#loading-layer").fadeOut("slow");
	});
	if(window.IS_ADMIN){
		
		jQuery(".div_admin_config_form").css("display","");
	}
});
//end

////////////////////////////////////////////////

function on_submit_login(){
	if (!getValueId('user_name') || !getValueId('password')){
		log_faile('Đăng nhập không thành công');
		jQuery('#overlay').click(function () {
			jQuery.unblockUI();
		});
		setTimeout(jQuery.unblockUI, 2000);
	}
	else{
		if(getValueId('set_cookie','checked')){
			var jset_cookie = 'on';
		}
		else{
			var jset_cookie = 'off';
		}

		jQuery.post(WEB_DIR+"ajax.php?act=user&code=login_user", {
			user: getValueId('user_name'),
			pass: getValueId('password'),
			set_cookie: jset_cookie},
			function(msg){
				if (msg == 'success'){
					location.reload();
				}
				else if (msg == 'unsuccess'){
					log_faile('Đăng nhập không thành công.');
				}
				else if (msg == 'nodata'){
					log_faile('Đăng nhập không thành công.');
				}
				else if (msg == 'un_active'){
					log_faile('Tài khoản của bạn chưa được kích hoạt!<br />Vui lòng kiểm tra lại email của mình!<br />hoặc <a href="'+BASE_URL+'reg_success.html&cmd=active">vào đây</a> để kích hoạt lại.',1000000);
				}
				else{
					//jQuery.blockUI({message: 'Bạn chưa nhập mật khẩu hoặc tên đăng nhập'});
					//log_faile('<div align="center" style="margin:-10px 0 0 -50px">Tài khoản của bạn bị khoá đến <div align="center" style="font-size:12px; padding-top:10px">'+msg+'</div></div>',10000);
					log_faile(msg,1000000);
					jQuery('#overlay').click(function () {							
						window.location.reload();							   
					});
					
					jQuery('#bound_log_faile').click(function () {							
						window.location.reload();							   
					});
				}
			}
			);
	}
}

function on_submit_login_this(){
	if (!getValueId('user_name_this') || !getValueId('password_this')){
		log_faile('Đăng nhập không thành công');
		jQuery('#overlay').click(function () {
			jQuery.unblockUI();
		});
		setTimeout(jQuery.unblockUI, 2000);
	}
	else{
		if(getValueId('set_cookie','checked')){
			var jset_cookie = 'on';
		}
		else{
			var jset_cookie = 'off';
		}

		jQuery.post(WEB_DIR+"ajax.php?act=user&code=login_user", {
			user: getValueId('user_name_this'),
			pass: getValueId('password_this'),
			set_cookie: jset_cookie},
			function(msg){
				if (msg == 'success'){
					location.reload();
				}
				else if (msg == 'unsuccess'){
					log_faile('Đăng nhập không thành công.');
				}
				else if (msg == 'nodata'){
					log_faile('Đăng nhập không thành công.');
				}
				else if(msg == 'un_active'){
					log_faile('Tài khoản của bạn chưa được kích hoạt!<br />Vui lòng kiểm tra lại email của mình!<br />hoặc <a href="'+BASE_URL+'reg_success.html&cmd=active">vào đây</a> để kích hoạt lại.',1000000);
				}
				else{
					//jQuery.blockUI({message: 'Bạn chưa nhập mật khẩu hoặc tên đăng nhập'});
					//log_faile('<div align="center" style="margin:-10px 0 0 -50px">Tài khoản của bạn bị khoá đến <div align="center" style="font-size:12px; padding-top:10px">'+msg+'</div></div>',10000);
					log_faile(msg,1000000);
					jQuery('#overlay').click(function () {							
						window.location.reload();							   
					});
					
					jQuery('#bound_log_faile').click(function () {							
						window.location.reload();							   
					});
				}
			}
		);
	}
}

function login_error(message){
	if(message)
	message_all = message ;
	else message_all='Bạn phải đăng nhập mới được dùng chức năng này';
	
	var str_openid = '';
	str_height = 99;

	if(OPENID_ON){
	/*	str_openid = '<div class="open_id_mini1" align="center"><div class="text_open_id">Chưa có tài khoản trên aznet ?</div><div class="btn_open_id" onclick="window.location=\''+OID_URL+'\'" onmouseout="this.className=\'btn_open_id\'" onmouseover="this.className=\'btn_open_id_hover\'"><a href="'+OID_URL+'">Đăng nhập nhanh với nick Y!M</a></div></div>';
		
		str_openid += '<div class="open_id_mini1" align="center"><div class="btn_open_id" onclick="window.location=\''+OID_URL+'\'" onmouseout="this.className=\'btn_open_id\'" onmouseover="this.className=\'btn_open_id_hover\'"><a href="'+OID_URL+'">Đăng nhập nhanh với Google</a></div></div>';*/
		
		str_openid  = '<div class="othrAcc" style="margin-left:70px;border-bottom:1px solid #cbcac8"> Đăng nhập dùng nick : <a class="google" href="'+OID_URL_GOG+'" title="Đăng nhập vào AzNetGroup bằng nick Google tại Google.com">Google</a> hoặc <a class="yahoo" href="'+OID_URL+'" title="Đăng nhập vào AzNetGroup bằng nick Yahoo tại Yahoo.com">Yahoo</a></div>';
		
		var str_height = 130;
	}
	jQuery.blockUI({message: '<div style="width:410px; border:1px solid #d1d4d3; background-color:#fff; padding:1px;" align="left"><div style=" height:26px; background-color:#695E4A" align="left"><span style=" line-height:26px;color: #fff; padding-left:10px;">Thông báo !</span><img src="style/images/i_close2.gif" width="13" height="13" id="close_box" title="Close..." style="cursor:pointer; padding:2px; margin-top:3px; _margin-top:0px; margin-left:300px; _margin-left:300px; position:absolute" /></div><div style=" background:url(style/images/bg_log_faile.gif) repeat-x bottom; min-height:99px;height:'+str_height+'px; "><div style="background:url(style/images/icon_log_login.gif) no-repeat 10px 10px; min-height:90px;_height:90px;"><div style="margin-left:76px; padding-top:20px;">'+message_all+'</div><div style="margin-top:10px;" align="center"><input type="button" name="sign_in" class="btnLogLogin"  id="login" value="Đăng nhập" /><input type="button" name="sign_in" class="btnLogOut"  id="no" value="Đóng" /></div></div>'+str_openid+'</div></div>', css: { border:'none', padding:0}});
	
	jQuery('#overlay').click(function () {
		jQuery.unblockUI();
	});
	jQuery('#login').mouseover(function () {
		jQuery(this).toggleClass("btnLogLoginOver");
		jQuery(this).removeClass("btnLogLogin");
	});
	jQuery('#login').mouseout(function () {
		jQuery(this).toggleClass("btnLogLogin");
		jQuery(this).removeClass("btnLogLoginOver");
	});
	jQuery('#no').mouseover(function () {
		jQuery(this).toggleClass("btnLogOutOver");
		jQuery(this).removeClass("btnLogOut");

	});
	jQuery('#no').mouseout(function () {
		jQuery(this).toggleClass("btnLogOut");
		jQuery(this).removeClass("btnLogOutOver");
	});

	jQuery('#close_box').click(function () {
		jQuery.unblockUI();
	});
	
	closeBlockUI();

	jQuery('#login').click(function() {
		// update the block message
		login_div();
	});

	jQuery('#no').click(function() {
		jQuery.unblockUI();
		return false;
	});
}

function login_div(){
	var str_openid = '';
	if(OPENID_ON){
		
		str_openid  = '<div class="othrAcc" style="margin-left:70px;border-bottom:1px solid #cbcac8"> Đăng nhập dùng nick : <a class="google" href="'+OID_URL_GOG+'" title="Đăng nhập vào AzNetGroup bằng nick Google tại Google.com">Google</a> hoặc <a class="yahoo" href="'+OID_URL+'" title="Đăng nhập vào AzNetGroup bằng nick Yahoo tại Yahoo.com">Yahoo</a></div>';
		
		var str_height = 130;
	}
	
	jQuery.blockUI({ message: '<form name="login_form" id="login_form" method="POST"><div style="box-shadow: 0 0 12px 0px rgba(0, 0, 0, 1);border-radius:5px;width:410px;border:8px solid #EBEBEB; background-color:#fff; padding:1px;" align="left"><div style=" height:32px;line-height: 32px; background-color:#695E4A;" align="left"><b style="color: #fff; padding-left:10px;">Đăng nhập !</b><img src="style/images/i_close2.gif" width="13" height="13" id="close_box" title="Close..." style="cursor:pointer; padding:2px; margin-top:8px; _margin-top:0px; margin-left:305px; _margin-left:300px; position:absolute" /></div><div id="loginForm" align="left" style=" background:url(style/images/bg_log_faile.gif) repeat-x bottom; height:178px"><div class="sign-in-field" style="padding-top:20px;"><label for="user_name" style="font-weight:normal; width:120px; color:#000">T&#234;n &#273;&#259;ng nh&#7853;p: </label><input name="user_name" type="text" id="user_name" /></div><div class="sign-in-field"><label for="password" style="font-weight:normal; width:120px;color:#000">M&#7853;t kh&#7849;u: </label><input name="password" type="password" id="password" /></div><div class="sign-in-set-cookie" align="left" style="margin-left:35px;font-weight:normal;" ><input id="set_cookie" name="set_cookie" value="on" type="checkbox" /><label for="set_cookie" class="cursor" style="font-weight:normal"> Ghi nhớ mật khẩu</label></div><div  class="sign-in-submit1" style="padding-left:122px;"><input type="submit" name="sign_in" class="btnLogLogin loginBtn floatLeft"  id="sign-in-submit"  value="Đăng nhập"/><span class="sign-in-lost-password"><a href="'+WEB_DIR+'forgot_password.html" style="font-weight:normal;">Qu&#234;n m&#7853;t kh&#7849;u?</a></span></div>'+str_openid+'</div></div></form>', css: { border:'none', padding:0,backgroundColor:'none'},overlayCSS:{ opacity:0.3, background:'#000000'} });

	jQuery('#sign-in-submit').mouseover(function () {
		jQuery(this).toggleClass("btnLogLoginOver");
		jQuery(this).removeClass("btnLogLogin");

	});
	jQuery('#sign-in-submit').mouseout(function () {
		jQuery(this).toggleClass("btnLogLogin");
		jQuery(this).removeClass("btnLogLoginOver");
	});
	jQuery("#login_form").submit(function(){
		on_submit_login();
		return false;
	});
			
	jQuery('#close_box').click(function () {
		jQuery.unblockUI();
	});
	
	closeBlockUI();
	
	jQuery('#overlay').click(function () {
		jQuery.unblockUI();
	});
}

function log_success(message_all,livetime){
	jQuery.blockUI({message: '<div style="width:360px; border:1px solid #d1d4d3; background-color:#fff; padding:1px;" align="left"><div style=" height:26px; background-color:#695E4A" align="left"><span style=" line-height:26px;color: #fff; padding-left:10px;">Thông báo !</span></div><div style=" background:url(style/images/bg_log_faile.gif) repeat-x bottom; min-height:119px;_height:119px;"><div style="background:url(style/images/icon_log_success.gif) no-repeat 10px 20px; min-height:119px;_height:119px;"><div style="margin-left:76px; padding-top:40px">'+message_all+'</div></div></div></div>', css: { border:'none', padding:0}});
	jQuery('div.blockUI').click(function () {
		jQuery.unblockUI();
	});
	if(livetime)
	livetime_all = livetime ;
	else livetime_all=2000;

	setTimeout(jQuery.unblockUI, livetime_all);
}


function log_faile(message_all,livetime){
	jQuery.blockUI({message: '<div id="bound_log_faile" style="width:360px;border:1px solid #d1d4d3; background-color:#fff; padding:1px;" align="left"><div style=" height:26px; background-color:#c12000" align="left"><span style=" line-height:26px;color: #fff; padding-left:10px;">Thông báo !</span></div><div style=" background:url(style/images/bg_log_faile.gif) repeat-x bottom; min-height:119px;_height:119px;"><div style="background:url(style/images/icon_log_faile.gif) no-repeat 10px 20px; min-height:119px;_height:119px;"><div style="margin-left:76px; padding-top:40px">'+message_all+'</div></div></div></div>', css: { border:'none', padding:0}});
	jQuery('div.blockUI').click(function () {
		jQuery.unblockUI();
	});
	if(livetime)
	livetime_all = livetime ;
	else livetime_all=2000;
	setTimeout(jQuery.unblockUI, livetime_all);
}

function mini_block_faile(id_block,message){
	jQuery(id_block).block({
		message: message,
		css: { border: '1px solid #f00',padding:'10px' }
	});
	setTimeout(function(){jQuery(id_block).unblock(); }, 2000);
}

function confirm_remove_email_alert(user_id,active_code,URL){
	var message_all='Bạn đang gửi yêu cầu tới hệ thống ngừng cung cấp email thông báo cho bạn?';
	jQuery.blockUI({message: '<div style="width:410px; border:1px solid #d1d4d3; background-color:#fff; padding:1px;" align="left"><div style=" height:26px; background-color:#695E4A" align="left"><span style=" line-height:26px;color: #fff; padding-left:10px;">Thông báo !</span></div><div style="background:url(style/images/icon_log_login.gif) no-repeat 10px 10px; min-height:99px;_height:99px;"><div style="margin-left:76px; padding-top:20px;">'+message_all+'</div><div style="margin-top:10px;" align="center"><input type="button" name="accept" class="btnLogLogin"  id="accept" value="Đồng Ý" />&nbsp;&nbsp;<input type="button" name="sign_in" class="btnLogOut"  id="no_thank" value="Hủy Bỏ" /></div></div></div>', css: { border:'none', padding:0}});
	jQuery('#overlay').click(function () {
		jQuery.unblockUI();
		window.location = URL;
	});
	jQuery('#no_thank').click(function() {
		jQuery.unblockUI();
		window.location = URL;
		return false;
	});

	jQuery('#accept').click(function() {
		jQuery.post(WEB_DIR+"ajax.php?act=user&code=remove_email_alert", {
			user_id: user_id,
			active_code: active_code
		},
		function(msg){
			if (msg != 'unsuccess'){
				window.location = URL;
			}
		}
		);
	})
}

function closeBlockUI(){		
	jQuery(window).keydown(function (e) {
      if (e.which == 27){
		  jQuery.unblockUI();
	  }
	});
}

function getValueId(id,type,svalue){
	if(document.getElementById(id)){
		
		if(typeof(type)=='undefined'){
			var type='value';
		}
		
		if(typeof(svalue)=='undefined'){
			var svalue='';
		}
		
		if(type=='value'){
			return document.getElementById(id).value;
		}
		else if(type=='checked'){
			return document.getElementById(id).checked;
		}
		else if(type=='assign'){
			return document.getElementById(id).value = svalue;
		}
		else{
			return '';
		}
	}
}

function MM_preloadImages() { //v3.0
	var d=document;
	if(d.images){
	if(!d.MM_p) d.MM_p=new Array();
	var i,j=d.MM_p.length,a=MM_preloadImages.arguments;
	for(i=0; i<a.length; i++)
	if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}


function getVar(href_val){
	var arr_view = new Array();
	if(href_val){
		var view = href_val.replace(/^.*#/, '');		
	}
	else{
		var view = (window && window.location && window.location.hash) ? window.location.hash : '#inbox';
		view = view.replace(/^.*#/, '');	
	}			
	arr_view = view.split('/');	
	return arr_view;	
}


Array.prototype.inArray = function (value){
	var i;
	for (i=0; i < this.length; i++) 
	{
		if (this[i] == value) 
		{
		return true;
		}
	}
	return false;
};


function Shuffle_arr(v){
	for(var j, x, i = v.length; i; j = parseInt(Math.random() * i), x = v[--i], v[i] = v[j], v[j] = x);
	return v;
}

function in_array(needle, haystack, argStrict)
{
    var key = '', strict = !!argStrict;
 
    if (strict) {
        for (key in haystack) {
            if (haystack[key] === needle) {
                return true;
            }
        }
    } else {
        for (key in haystack) {
            if (haystack[key] == needle) {
                return true;
            }
        }
    }
 
    return false;
}

function overlay(curobj, subobjstr, opt_position)
{
	if (document.getElementById)
	{
		var subobj=document.getElementById(subobjstr)
		
		subobj.style.display=(subobj.style.display!="block")? "block" : "none"
		
		var xpos=getposOffset(curobj, "left")+((typeof opt_position!="undefined" && opt_position.indexOf("right")!=-1)? -(subobj.offsetWidth-curobj.offsetWidth) : 0) 
		var ypos=getposOffset(curobj, "top")+((typeof opt_position!="undefined" && opt_position.indexOf("bottom")!=-1)? curobj.offsetHeight : 0)
	
		if((typeof opt_position!="undefined" && opt_position.indexOf("top")!=-1))
		ypos=getposOffset(curobj, "top")-subobj.offsetHeight-3;
	
		subobj.style.left=xpos+"px";
		subobj.style.top=ypos+"px";
		return false
	}
	else
		return true
}

function overlayclose(subobj){
	if(document.getElementById(subobj)){
		document.getElementById(subobj).style.display="none";
	}
}


function getposOffset(overlay, offsettype)
{
	var totaloffset		=(offsettype=="left")? overlay.offsetLeft : overlay.offsetTop;
	var parentEl		=overlay.offsetParent;
	
	while (parentEl!=null)
	{
		totaloffset=(offsettype=="left")? totaloffset+parentEl.offsetLeft : totaloffset+parentEl.offsetTop;
		parentEl=parentEl.offsetParent;
	}
	
	return totaloffset;
}

function number_format(Num)
{
	Num = Num.toString().replace(/^0+/,"").replace(/\./g,"").replace(/,/g,"");// Bỏ hết số 0 ở đầu dãy số | Bỏ hết dấu . trong dãy số
	Num = "" + parseInt(Num);
	var temp1 = "";
	var temp2 = "";
	
	if (Num == 0 || Num == '0' || Num == '' || isNaN(Num)) {
		return 0;
	}
	else { 
		//if (end.length == 2) end += "0";
		//if (end.length == 1) end += "00";
		//if (end == "") end += ",00";
		var count = 0;
		for (var k = Num.length-1; k >= 0; k--) {
			var oneChar = Num.charAt(k);
			if (count == 3) {
				temp1 += ".";
				temp1 += oneChar;
				count = 1;
				continue;
			}
			else{
				temp1 += oneChar;
				count ++;
			}
		}
		
		for (var k = temp1.length-1; k >= 0; k--) {
			var oneChar = temp1.charAt(k);
			temp2 += oneChar;
		}
		//temp2 = temp2 + end;
		return temp2;
	}
}


function show_offer_tab(obj, catid, tab_class, preid)
{
	jQuery('.'+tab_class).removeClass('offeractive');
	
	jQuery(obj).addClass('offeractive');
	
	jQuery('.' + preid + 'content').hide();
	
	jQuery('#'+preid + catid).show();
}
var cart_items 	= '';
var cart_arr	= [];

jQuery(document).ready(function(){
	//:button :submit :reset
/*	jQuery(':button').mouseover(function(){
		if(this.id != 'search_btn')			
		jQuery(this).css({'background':'url(style/img/button_bg.png) 0 -23px repeat-x','border':'1px solid #CD0000','color':'#fff'});	
	});
	
	jQuery(':button').mouseout(function(){
		if(this.id != 'search_btn')			
		jQuery(this).css({'background':'url(style/img/button_bg.png) 0 0 repeat-x','border':'1px solid #C2C2C2','color':'#000'});				
	});
	
	jQuery(':submit').mouseover(function(){
		jQuery(this).css({'background':'url(style/img/button_bg.png) 0 -23px repeat-x','border':'1px solid #CD0000','color':'#fff'});	
	});
	
	jQuery(':submit').mouseout(function(){							
		jQuery(this).css({'background':'url(style/img/button_bg.png) 0 0 repeat-x','border':'1px solid #C2C2C2','color':'#000'});				
	});
	
	jQuery(':reset').mouseover(function(){
		jQuery(this).css({'background':'url(style/img/button_bg.png) 0 -23px repeat-x','border':'1px solid #CD0000','color':'#fff'});	
	});
	
	jQuery(':reset').mouseout(function(){
		jQuery(this).css({'background':'url(style/img/button_bg.png) 0 0 repeat-x','border':'1px solid #C2C2C2','color':'#000'});				
	});
	*/
	cart_items 	= jQuery.cookie('cart_items');//ID các SP cho vào giỏ hàng
	
	if(!cart_items)
	cart_items = '';
	
	if(cart_items != '')
	{
		cart_arr = cart_items.split(',');
		
		jQuery('#cart_num').html('' + parseInt(cart_arr.length) + '');
	}	
});

function add_to_cart(p_id, price)
{
	if(price > 0)
	{
		if(cart_items != '')
		{
			for(var i in cart_arr)
			{
				if(p_id == cart_arr[i])
				{
					if(confirm('Sản phẩm này đã có thêm trong giỏh hàng! Bạn có muốn xem giỏ hàng và cập nhật cho giỏ hàng không?'))
					{
						window.location = WEB_DIR + 'cart.html';
					}
					
					return false;
				}
			}
		}
		
		cart_items += (cart_items != '' ? ',' : '') + p_id;
		cart_arr[cart_arr.length] = p_id;
		
		jQuery('#cart_num').html('' + parseInt(cart_arr.length) + '');

		//set cookie theo ngay
		jQuery.cookie('cart_items', cart_items, {path: '/', expires: 365});
		
		log_success("Đã thêm sản phẩm vào giỏ hàng!");
	}
	else
	{
		log_faile('Hãy liên hệ với chúng tôi để có giá sản phẩm và mua sản phẩm này!');
		return false;
	}
}

function auto_resize(obj_id, total_item, intCol)
{
	var intMax;
	var intRow = Math.ceil(total_item/intCol);//Số dòng
	
	var intCount 	= 0;//ID hiện tại
	var pos 		= 1;//Vị trí hiện tại
	
	//alert(total_item);alert(intCol);
	//alert(obj_id);
	
	for(var i = 1; i <= intRow; i++)
	{
		intMax = 0;
	
		for(var j=1; j<= intCol; j++)
		{
			intCount++;
		
			if(intCount <= total_item)
			{
				if(document.getElementById(obj_id + intCount))
				{
					var intH = parseInt(document.getElementById(obj_id + intCount).offsetHeight);
				}
				
				if (intH>intMax)
				{
					intMax = intH;
				}
			}
			else
			{
				intCount = total_item;
				
				break;
			}
		}
		
		for(var j = pos; j<= intCount ; j++)
		{
			jQuery('#' + obj_id + j).css('height',intMax + 'px');
		}
		
		pos = intCount + 1;
	}
}