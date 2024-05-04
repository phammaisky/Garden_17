if (typeof eCache == 'undefined') {
    var eCache = { version:"1.0.0", date:"31.01.2012", author:"Ngannv", maxSize:500, keys:new Array(), cache_length:0, items:new Array(), removeItem:function (pKey)
    {
        var tmp;
        if (typeof(eCache.items[pKey]) != 'undefined') {
            eCache.cache_length--;
            tmp = eCache.items[pKey];
            delete eCache.items[pKey];
        }
        return tmp;
    }, getItem:function (pKey)
    {
        return eCache.items[pKey];
    }, hasItem:function (pKey)
    {
        return typeof(eCache.items[pKey]) != 'undefined';
    }, removeOldestItem:function ()
    {
        eCache.removeItem(eCache.keys.shift());
    }, clear:function ()
    {
        var tmp = eCache.cache_length;
        eCache.keys = new Array();
        eCache.cache_length = 0;
        eCache.items = new Array();
        return tmp;
    } }
}
eCache.setItem = function (pKey, pValue)
{
    if (typeof(pValue) != 'undefined') {
        if (typeof(eCache.items[pKey]) == 'undefined') {
            eCache.cache_length++;
        }
        eCache.keys.push(pKey);
        eCache.items[pKey] = pValue;
        if (eCache.cache_length > eCache.maxSize) {
            eCache.removeOldestItem();
        }
    }
    return pValue;
};

