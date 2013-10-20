ko.numericObservable = function (initialValue) {
    var _actual = ko.observable(initialValue);

    var result = ko.dependentObservable({
        read: function () {
            return _actual();
        },
        write: function (newValue) {
            var parsedValue = parseFloat(newValue);
            _actual(isNaN(parsedValue) ? null : parsedValue);
        }
    });

    return result;
};


function Fixture(id, date, homeTeamId, awayTeamId, homeTeamGoals, awayTeamGoals) {
    var self = this;
    self.fixtureId = id;
    self.date = ko.observable(moment(date).toDate());
    self.homeTeamId = homeTeamId;
    self.awayTeamId = awayTeamId;
    self.homeTeamGoals = ko.numericObservable(homeTeamGoals);
    self.awayTeamGoals = ko.numericObservable(awayTeamGoals);
}

// Overall viewmodel for this screen, along with initial state
function DayModel(name, date, coefficient, fixtures, teams) {
    var self = this;

    self.name = ko.observable(name);
    self.date = ko.observable(moment(date).toDate());
    self.coefficient = ko.observable(coefficient);
    self.fixtures = ko.observableArray(ko.utils.arrayMap(fixtures, function (f) {
        return new Fixture(f.FixtureId, f.Date, f.HomeTeamId, f.AwayTeamId, f.HomeTeamGoals, f.AwayTeamGoals);
    }));

    self.teams = teams;

    //// Operations
    self.addFixture = function () {
        self.fixtures.push(new Fixture(0, new Date(), null, null, null, null));
    }
    self.removeFixture = function (fixture) { self.fixtures.remove(fixture) }
    self.save = function () {
        var result = {
            Name: self.name(),
            Date: self.date(),
            Coefficient: self.coefficient(),
            Fixtures: ko.toJS(self.fixtures)
        };
        var json = ko.toJSON(result);
        ko.utils.postJson(location.href, { dayForm: result } );
    }
}