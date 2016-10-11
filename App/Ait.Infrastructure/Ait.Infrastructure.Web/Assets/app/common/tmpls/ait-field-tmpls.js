
angular.module("aitFieldTmpls", []).run(["$templateCache", function ($templateCache) {

    
    
    $templateCache.put("Assets/app/common/tmpls/ait-field-text.html", '\
<div class="input-field ait-field-text" ng-if="::$ctrl.type">\
    <input id="{{::$ctrl.name}}" name="{{::$ctrl.name}}"\
           type="text"\
           ng-model="$ctrl.bindModel"\
    ng-init="$ctrl.init()"\
    ng-class="{invalid: !$ctrl.form[$ctrl.name].$valid, valid: $ctrl.form[$ctrl.name].$valid}"\
    ait-valid="{{$ctrl.valid}}"\
    autocomplete="{{::($ctrlautocomplete||\'off\')}}"\
    ait-mask-placeholder="{{::$ctrl.placeholder}}"\
    ait-mask="{{::$ctrl.aitFieldMask}}"\
    ng-required="$ctrl.required" />\
<ait-field-error ng-show="!$ctrl.form[$ctrl.name].$valid">\
 <span ng-if="$ctrl.form[$ctrl.name].$error.required">Поле обязательно для заполнения</span>\
 <span ng-if="!$ctrl.form[$ctrl.name].$error.required">значение некорректно</span>\
</ait-field-error>\
<label for="{{::$ctrl.name}}" ng-bind="::$ctrl.caption" class="caption"></label>\
</div>\
');    
    
          
    $templateCache.put("Assets/app/common/tmpls/ait-field-fio.html",'\
<div class="input-field ait-field-text" ng-if="::$ctrl.type">\
    <input id="{{::$ctrl.name}}" name="{{::$ctrl.name}}"\
    type="text"\
    ng-model="$ctrl.bindModel"\
    ng-class="{invalid: !$ctrl.form[$ctrl.name].$valid, valid: $ctrl.form[$ctrl.name].$valid}"\
    ait-valid="fio"\
    autocomplete="{{::($ctrlautocomplete||\'off\')}}"\
    ng-init="$ctrl.init()"\
    ait-mask-placeholder="{{::$ctrl.placeholder}}"\
    ng-required="$ctrl.required" />\
<ait-field-error ng-show="!$ctrl.form[$ctrl.name].$valid">\
 <span ng-if="$ctrl.form[$ctrl.name].$error.required">Поле обязательно для заполнения</span>\
 <span ng-if="!$ctrl.form[$ctrl.name].$error.required">только русские буквы</span>\
</ait-field-error>\
<label for="{{::$ctrl.name}}" ng-bind="::$ctrl.caption"  class="caption"></label>\
</div>\
');


    $templateCache.put("Assets/app/common/tmpls/ait-field-date.html", '\
    <div class="input-field ait-field-date" ng-if="::$ctrl.type">\
        <input id="{{::$ctrl.name}}" type="text" name="{{::$ctrl.name}}"\
               ng-model="$ctrl.bindModel"\
               ng-class="{invalid: !$ctrl.form[$ctrl.name].$valid, valid: $ctrl.form[$ctrl.name].$valid}"\
               ait-mask="date"\
               ng-init="$ctrl.init()"\
               autocomplete="{{::($ctrlautocomplete||\'off\')}}"\
               ait-valid="{{$ctrl.valid}}"\
               ng-required="$ctrl.required" />\
        <ait-field-error ng-show="!$ctrl.form[$ctrl.name].$valid">\
            <span ng-if="$ctrl.form[$ctrl.name].$error.required">Поле обязательно для заполнения</span>\
            <span ng-if="!$ctrl.form[$ctrl.name].$error.required">значение некорректно</span>\
        </ait-field-error>\
        <label for="{{::$ctrl.name}}" class="caption" ng-bind="::$ctrl.caption"></label>\
    </div>\
    ');


    $templateCache.put("Assets/app/common/tmpls/ait-field-email.html", '\
<div class="input-field ait-field-email" ng-if="::$ctrl.type">\
    <input id="{{::$ctrl.name}}" name="{{::$ctrl.name}}"\
           type="email"\
           ng-model="$ctrl.bindModel"\
    ng-class="{invalid: !$ctrl.form[$ctrl.name].$valid, valid: $ctrl.form[$ctrl.name].$valid}"\
    ait-valid="email"\
    ng-init="$ctrl.init()"\
    autocomplete="{{::($ctrlautocomplete||\'off\')}}"\
    ng-required="$ctrl.required" />\
<ait-field-error ng-show="!$ctrl.form[$ctrl.name].$valid">\
 <span ng-if="$ctrl.form[$ctrl.name].$error.required">Поле обязательно для заполнения</span>\
 <span ng-if="!$ctrl.form[$ctrl.name].$error.required">значение некорректно</span>\
</ait-field-error>\
<label for="{{::$ctrl.name}}" ng-bind="::$ctrl.caption"  class="caption"></label>\
</div>\
');


    $templateCache.put("Assets/app/common/tmpls/ait-field-item.html",
        '<span class="ait-field-item" ng-bind="::item.Value||item.value||item.Text||item.text||item"></span>');


    $templateCache.put("Assets/app/common/tmpls/ait-field-kladr.html", '\
<div class="row">\
    <div class="col s12 for-tab">\
        Адрес регистрации\
    </div>\
    <div class="input-field col s12">\
        <div style="color:#9e9e9e;">Выберите регион:</div>\
        <region-choose region-id="$ctrl.regionId" on-selected="$ctrl.state.onRegionSelected(item)"></region-choose>\
    </div>\
    <div>&nbsp;</div>\
    <div class="input-field col s12">\
        <input type="text" id="street" name="street" autocomplete="off" kladr-street="$ctrl.kladr" kladr-street-region="$ctrl.regionId" ng-model="kladr" required />\
        <label for="street">Адрес</label>\
    </div>\
    {{$ctrl.kladr}}\
    </div>\
    <div class="input-field ait-field-text" ng-if="::$ctrl.type">\
        <input id="{{::$ctrl.name}}" name="{{::$ctrl.name}}"\
               type="text"\
               ng-init="$ctrl.init()"\
               ng-model="$ctrl.bindModel"\
               ng-class="{invalid: !$ctrl.form[$ctrl.name].$valid, valid: $ctrl.form[$ctrl.name].$valid}"\
               ait-valid="{{$ctrl.valid}}"\
               ng-required="$ctrl.required" />\
        <ait-field-error ng-show="!$ctrl.form[$ctrl.name].$valid">\
            <span ng-if="$ctrl.form[$ctrl.name].$error.required">Поле обязательно для заполнения</span>\
            <span ng-if="!$ctrl.form[$ctrl.name].$error.required">значение некорректно</span>\
        </ait-field-error>\
        <label for="{{::$ctrl.name}}" ng-bind="::$ctrl.caption"  class="caption"></label>\
</div>\
');


    $templateCache.put("Assets/app/common/tmpls/ait-field-phone.html", '\
<div class="input-field ait-field-phone" ng-if="::$ctrl.type">\
    <input id="{{::$ctrl.name}}" name="{{::$ctrl.name}}"\
           type="text"\
           ng-model="$ctrl.bindModel"\
    ng-init="$ctrl.init()"\
    ng-class="{invalid: !$ctrl.form[$ctrl.name].$valid, valid: $ctrl.form[$ctrl.name].$valid}"\
    autocomplete="{{::($ctrlautocomplete||\'off\')}}"\
    ait-mask="phone"\
    ait-valid="phone"\
    ng-required="$ctrl.required" />\
<ait-field-error ng-show="!$ctrl.form[$ctrl.name].$valid">\
 <span ng-if="$ctrl.form[$ctrl.name].$error.required">Поле обязательно для заполнения</span>\
 <span ng-if="!$ctrl.form[$ctrl.name].$error.required">значение некорректно</span>\
</ait-field-error>\
<label for="{{::$ctrl.name}}" ng-bind="::$ctrl.caption"  class="caption"></label>\
</div>\
');

    $templateCache.put("Assets/app/common/tmpls/ait-field-radio.html", '\
<div ng-if="::$ctrl.type" class="ait-field-radio">\
    <label for="{{::$ctrl.name}}" ng-bind="::$ctrl.caption"></label>\
    <div ng-if="$ctrl.itemsSource" class="ait-field-items">\
        <p ng-repeat="item in ::$ctrl.itemsSource">\
            <input name="{{::$ctrl.name}}" type="radio" id="{{::$ctrl.name + \'_\' + $index}}" ng-model="$ctrl.bindModel" ng-value="::item" />\
            <label for="{{::$ctrl.name + \'_\' + $index}}">\
                <ait-field-item item="item" template="{{::$ctrl.aitFieldItemTemplate}}">\
                </ait-field-item>\
            </label>\
        </p>\
    </div>\
    <ait-field-error ng-if="$ctrl.required && $ctrl.bindModel==null">\
        <span>Поле обязательно для заполнения</span>\
    </ait-field-error>\
</div>\
');

    $templateCache.put("Assets/app/common/tmpls/ait-field-region.html", '\
<div class="ait-field-region" ng-if="::$ctrl.type">\
    <div id="{{::$ctrl.name}}" name="{{::$ctrl.name}}">\
        <div>\
            <label ng-bind="::$ctrl.caption"></label>\
        </div>\
        <ait-region-choose region-id="$ctrl.bindModel">\
        </ait-region-choose>\
    </div>\
    <ait-field-error ng-show="$ctrl.required && !$ctrl.bindModel">\
        <span>Регион обязательный параметр</span>\
    </ait-field-error>\
</div>\
');

    $templateCache.put("Assets/app/common/tmpls/ait-field-street.html", '\
<div class="input-field ait-field-street" ng-if="::$ctrl.type">\
    <input id="{{::$ctrl.name}}" name="{{::$ctrl.name}}"\
    type="text"\
    ait-kladr-street-region="$ctrl.parentBind"\
    ait-kladr-street="$ctrl.Kladr"\
    ng-init="$ctrl.init()"\
    ng-model="$ctrl.bindModel"\
    ng-class="{invalid: !$ctrl.form[$ctrl.name].$valid, valid: $ctrl.form[$ctrl.name].$valid}"\
    ait-valid="{{$ctrl.valid}}"\
    ait-field-tag ="{{::$ctrl.aitFieldTag}}"\
    autocomplete="{{::($ctrlautocomplete||\'off\')}}"\
    ng-required="$ctrl.required" />\
    <ait-field-error ng-show="!$ctrl.form[$ctrl.name].$valid">\
        <span ng-if="$ctrl.form[$ctrl.name].$error.required">Поле обязательно для заполнения</span>\
        <span ng-if="!$ctrl.form[$ctrl.name].$error.required">значение некорректно</span>\
    </ait-field-error>\
    <label for="{{::$ctrl.name}}" ng-bind="::$ctrl.caption" class="caption"></label>\
</div>\
');

}]);