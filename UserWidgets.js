/**
 * This wisget module provides PBG the user widgets functionaltiy as sorting, adding, removing
 * Author: Ahmad Harb
 * Version: 1.0
 * Created: 22 Jan, 2015
 * Updated: 26 Feb, 2015
 */
var userWidgets = (function () {

    // Widget class
    function Widget(WidgetType, Order, PlaceholderOrder) {
        this.WidgetType = WidgetType;
        this.Order = Order;
        this.PlaceholderOrder = PlaceholderOrder;
    };


    var _sort = function (event, ui) {

        var sortedWidgets = [];

        var $item = ui.item;
        var newIndex = $item.index();

        var $widgets = $(".portlet").not(".widget-removed");

        $widgets.each(function (index, element) {

            var $element = $(element);

            var order = $element.index();
            var widgetType = $element.attr("data-widget-type");
            var placeholderOrder = $element.parent().attr("data-widget-placeholder-order");

            var widgetOb = new Widget(widgetType, order, placeholderOrder);

            sortedWidgets.push(widgetOb);


        });

        var result = JSON.stringify(sortedWidgets);

        // call api to save the changes
        _save(result);

    };

    var _add = function (widgetTypeVal) {

        var dataOb = { widgetType: widgetTypeVal }

        // code to send the widget type to remove...
        $.ajax({
            type: "POST",
            url: "/webservices/widgets.asmx/Add",
            data: JSON.stringify(dataOb),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
               
                var $widgets = $(".portlet").not(".widget-removed");
                var sortedWidgets = [];

                // Resorting Widgets
                $widgets.each(function (index, element) {

                    var $element = $(element);

                    var order = $element.index();
                    var widgetType = $element.attr("data-widget-type");
                    var placeholderOrder = $element.parent().attr("data-widget-placeholder-order");

                    var widgetOb = new Widget(widgetType, order, placeholderOrder);

                    sortedWidgets.push(widgetOb);


                });

                var result = JSON.stringify(sortedWidgets);

                // call api to save the changes
                _save(result);
            }
        });
    };

    var _remove = function (element) {

        var $element = $(element);

        var widgetToDelete = $element.closest('[data-widget-type]');

        var widgetTypeVal = widgetToDelete.attr('data-widget-type');

        var dataOb = { widgetType: widgetTypeVal };

        // code to send the widget type tp remove...
        $.ajax({
            type: "POST",
            url: "/webservices/widgets.asmx/Remove",
            data: JSON.stringify(dataOb),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log("Remove done from server!");
            }
        });
    };

    var _save = function (result) {


        $.ajax({
            type: "POST",
            data: "widgets=" + result,
            url: "/webservices/widgets.asmx/Sort",
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            success: function (response) {
                console.log("Sort done from server!");
            }
        });


    }


    return {
        sort: _sort,
        add: _add,
        remove: _remove
    };

})();