function parseDate(str, utcmode, xdate) {
    var m = str.match(/^(\d{2})[-/](\d{2})[-/](\d{4})( (\d{2}):(\d{2})(:(\d{2}))?)?$/);
    if (m) {
        var d = new Date(
            m[3],
            m[2] ? m[2] - 1 : 0,
            m[1] || 1,
            m[5] || 0,
            m[6] || 0,
            m[8] || 0
        );
        return xdate.setTime(+d);
    }
}

XDate.parsers.push(parseDate);

ko.bindingHandlers.date = {
    init: function (elem, valueAccessor, allBindingsAccessor) {
        var dateConverter = ko.computed({
            read: function () {
                var value = valueAccessor(),
                    allBindings = allBindingsAccessor();
                console.log('Writing ' + value());
                return (new XDate(value())).toString(allBindings.dateFormat);
            },
            write: function (value) {
                console.log('Reading ' + value);
                valueAccessor()((new XDate(value)).toISOString());
                console.log('viewModel.date: ' + viewModel.date());
            }
        });

        ko.applyBindingsToNode(elem, { value: dateConverter });
    }
}