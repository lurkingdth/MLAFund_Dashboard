!function(e,n){"object"==typeof exports&&"undefined"!=typeof module?module.exports=n():"function"==typeof define&&define.amd?define(n):((e=e||self).__vee_validate_locale__sk=e.__vee_validate_locale__sk||{},e.__vee_validate_locale__sk.js=n())}(this,function(){"use strict";var e,n={name:"sk",messages:{after:function(e,n){var o=n[0];return"Položka "+e+" musí byť vačšia "+(n[1]?"alebo rovná ":"")+" ako položka "+o},alpha:function(e){return e+" môže obsahovať len písmená"},alpha_dash:function(e){return e+" môže obsahovať len písmená, číslice, bodky a podčiarknutie"},alpha_num:function(e){return e+" môže obsahovať len písmená a číslice"},alpha_spaces:function(e){return e+" môže obsahovať len písmená, číslice a medzery"},before:function(e,n){var o=n[0];return"Položka "+e+" musí byť menšia "+(n[1]?"alebo rovná ":"")+" ako položka "+o},between:function(e,n){return"Položka "+e+" musí byť medzi "+n[0]+" a "+n[1]},confirmed:function(e){return"Hodnota položky "+e+" nie je rovnaká"},credit_card:function(e){return"Položka "+e+" je neplatná"},date_between:function(e,n){return e+" musí byť medzi "+n[0]+" a "+n[1]},date_format:function(e,n){return e+" musí byť vo formáte "+n[0]},decimal:function(e,n){void 0===n&&(n=[]);var o=n[0];return void 0===o&&(o="*"),"Položka "+e+" musí byť číselná a smie obsahovať"+(o&&"*"!==o?" "+o:"")+" desatinné miesta"},digits:function(e,n){var o=n[0];return"Položka "+e+" musí obsahovať "+o+" "+(o<5?"čísla":"čísiel")},dimensions:function(e,n){return"Položka "+e+" musí mať "+n[0]+" x "+n[1]+" pixlov"},email:function(e){return"Položka "+e+" musí obsahovať správnu emailovú adresu"},excluded:function(e){return"Položka "+e+" má nesprávnu hodnotu"},ext:function(e){return e+" nie je platný súbor"},image:function(e){return e+" nie je obrázok"},included:function(e){return"Položka "+e+" má nesprávnu hodnotu"},ip:function(e){return"Položka "+e+" nie je platná IP adresa"},max:function(e,n){return"Položka "+e+" môže obsahovať najviac "+n[0]+" znakov"},max_value:function(e,n){return"Položka "+e+" musí byť maximálne "+n[0]},mimes:function(e){return"Položka "+e+" obsahuje nesprávny typ súboru"},min:function(e,n){var o=n[0];return"Položka "+e+" musí obsahovať minimálne "+o+" "+(o<4?"znaky":"znakov")},min_value:function(e,n){return"Položka "+e+" musí byť minimálne "+n[0]},numeric:function(e){return"Položka "+e+" môže obsahovať len číslice"},regex:function(e){return"Formát položky "+e+" je nesprávny"},required:function(e){return"Položka "+e+" je povinná"},size:function(e,n){return"Položka "+e+" musí byť menej ako "+function(e){var n=1024,o=0===(e=Number(e)*n)?0:Math.floor(Math.log(e)/Math.log(n));return 1*(e/Math.pow(n,o)).toFixed(2)+" "+["Byte","KB","MB","GB","TB","PB","EB","ZB","YB"][o]}(n[0])},url:function(e){return"Položka "+e+" neobsahuje platnú URL"}},attributes:{}};return"undefined"!=typeof VeeValidate&&VeeValidate.Validator.localize(((e={})[n.name]=n,e)),n});