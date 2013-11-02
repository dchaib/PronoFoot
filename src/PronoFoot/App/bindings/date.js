ko.bindingHandlers.date = {
    init: function (elem, valueAccessor, allBindingsAccessor) {
        var dateFormat = allBindingsAccessor().dateFormat || 'L';
        var dateConverter = ko.computed({
            read: function () { return moment(valueAccessor()()).format(dateFormat); },
            write: function (value) {
                valueAccessor()(moment(value, dateFormat).toDate());
            }
        });

        ko.applyBindingsToNode(elem, {
            value: dateConverter
        });
    }
};