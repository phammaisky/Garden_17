var Effect_Array = new Array('puff', 'explode', 'bounce', 'shake'); // new Array('blind', 'drop', 'puff', 'slide'); //new Array('blind', 'bounce', 'drop', 'explode', 'puff', 'scale', 'shake', 'size', 'slide'); //new Array('blind', 'bounce', 'clip', 'drop', 'explode', 'fade', 'fold', 'highlight', 'puff', 'pulsate', 'scale', 'shake', 'size', 'slide', 'transfer');
var Effect_Hide_Tab_Array = new Array('blind', 'bounce', 'drop', 'explode', 'puff', 'scale', 'shake', 'size', 'slide'); //'clip', 

function Effect_Option(Option_For, selectedEffect) {

    var options = {};

    if (selectedEffect === "blind") {
        options = { direction: 'down' };
    } else
        if (selectedEffect === "bounce") {
            options = { distance: 50, times: 5 };
        } else
            if (selectedEffect === "clip") {
                options = { direction: 'up' };
            } else
                if (selectedEffect === "drop") {
                    options = { direction: 'down' };
                } else
                    if (selectedEffect === "explode") {
                        if (Option_For == 'Page_Background_Picture') {
                            options = { pieces: 100 };
                        }
                        else {
                            options = { pieces: 25 };
                        }
                    } else
                        if (selectedEffect === "fade") {
                        }
                        else
                            if (selectedEffect === "fold") {
                                options = { size: 15, horizFirst: false };
                            }
                            else
                                if (selectedEffect === "highlight") {
                                    options = { color: "#ffff99" };
                                }
                                else
                                    if (selectedEffect === "puff") {
                                        options = { percent: 100 };
                                    }
                                    else
                                        if (selectedEffect === "pulsate") {
                                            options = { times: 5 };
                                        }
                                        else
                                            if (selectedEffect === "scale") {
                                                options = { direction: 'both', origin: ["middle", "center"], percent: 0, scale: 'both' };
                                            }
                                            else
                                                if (selectedEffect === "shake") {
                                                    options = { direction: 'left', distance: 50, times: 5 };
                                                }
                                                else
                                                    if (selectedEffect === "size") {
                                                        options = { to: { width: 0, height: 0} };
                                                    }
                                                    else
                                                        if (selectedEffect === "slide") {
                                                            options = { direction: 'down' }; //, distance: 200 
                                                        }
                                                        else
                                                            if (selectedEffect === "transfer") {
                                                                //options = { to: "#Friend_Banner_tbl", className: "ui-effects-transfer" };
                                                            }

    //
    return options;
}


function Run_Effect_Hide_Background(ID) {

    var Random_Number = Math.random();
    var Random_Number_In_Effect_Array = Math.floor(window.Effect_Array.length * Random_Number);

    //
    var selectedEffect = 'puff'; //window.Effect_Array[Random_Number_In_Effect_Array];
    var options = Effect_Option('Page_Background_Picture', selectedEffect);

    //
    $('#' + ID).hide(selectedEffect, options, 2500);    
}