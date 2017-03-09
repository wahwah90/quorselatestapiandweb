var recommendedUrl = domainUri + "/nonrest/courses";
var timeTickerRecommendedListUrl = recommendedUrl + "/gettimetickers";
var popularRecommendedListUrl = recommendedUrl + "/getpopularlazyloadingasync";
var latestRecommendedListUrl = recommendedUrl + "/getlatestlazyloadingasync";
var suggestionRecommendedListUrl = recommendedUrl + "/getsuggestionlazyloadingasync";
var timeTickerViewModel; //view model with extend methods
var popularViewModel; //view model with extend methods
var latestViewModel; //view model with extend methods
var suggestionViewModel; //view model with extend methods
//time timer loading
var renderedHandler = function () {
    model.preload(false);
    initCountDown();
}
var renderedHandlerPopular = function () {
    model.preloadPopular(false);
    initCountDown();
}
var renderedHandlerLatest = function () {
    model.preloadLatest(false);
    initCountDown();
}
var renderedHandlerSuggestion = function () {
    model.preloadSuggestion(false);
    initCountDown();
}
//extend methods
var extendModel = function (data) {
    ko.mapping.fromJS(data, {}, this);
    this.rating = ko.computed(function () {
        return this.CourseRating().toFixed(1);
    }, this);
    this.normalPrice = ko.computed(function () {
        if (typeof this.ClassSimpleModel.CourseHighestClassPrice !== 'undefined')
        return this.ClassSimpleModel.CourseHighestClassPrice().toFixed(2);
    }, this);
    this.discountedPrice = ko.computed(function () {
        if (typeof this.ClassSimpleModel.CourseLowestClassPrice !== 'undefined')
            return this.ClassSimpleModel.CourseLowestClassPrice().toFixed(2);
        else
            return this.CoursePrice().toFixed(2);
    }, this);
    this.discountedRate = ko.computed(function () {
        if (typeof this.ClassSimpleModel.CourseDiscountRate !== 'undefined')
        {
            return this.ClassSimpleModel.CourseDiscountRate().toFixed(0);
        }      
    }, this);
    this.ttEndDate = ko.computed(function () {
        if (this.CourseClassTTEndDate())
        {
            var date = new Date(this.CourseClassTTEndDate());
            var ttDate = date.setDate(date.getDate() + 1);
            return new Date(ttDate).yyyymmdd();
        }
       
    }, this);
    this.isTimeTicker = ko.computed(function () {
        if (this.CourseClassTTEndDate())
            return true;
        else
            return false;
    },this);
};


var recommendedOptions = {
    create: function (options) {
        return new extendModel(options.data);
    }

};

var Mapping = function (data) {
    var items = ko.mapping.fromJS(data, recommendedOptions);
    return items;
}

var getTimeTickerRecommendedList = function () {
    sendRequest(timeTickerRecommendedListUrl, "GET", null, function (data) {
        model.timeTickerRecommended.removeAll();
        model.timeTickerRecommended.push.apply(model.timeTickerRecommended, data);
        timeTickerViewModel = Mapping(model.timeTickerRecommended());//mapping to extend methods or properties
        model.timeTickerRecommendedViewModel.push.apply(model.timeTickerRecommendedViewModel, timeTickerViewModel());
       
    }, null, { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' });
}

var getPopularRecommendedList = function () {
    sendRequest(popularRecommendedListUrl, "GET", null, function (data) {
        model.popularRecommended.removeAll();
        model.popularRecommended.push.apply(model.popularRecommended, data);
        popularViewModel = Mapping(model.popularRecommended());//mapping to extend methods or properties
        model.popularRecommendedViewModel.push.apply(model.popularRecommendedViewModel, popularViewModel());

    }, null, { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' });
}

var getLatestRecommendedList = function () {
    sendRequest(latestRecommendedListUrl, "GET", null, function (data) {
        model.latestRecommended.removeAll();
        model.latestRecommended.push.apply(model.latestRecommended, data);
        latestViewModel = Mapping(model.latestRecommended());//mapping to extend methods or properties
        model.latestRecommendedViewModel.push.apply(model.latestRecommendedViewModel, latestViewModel());

    }, null, { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' });
}

var getSuggestionRecommendedList = function () {
    sendRequest(suggestionRecommendedListUrl, "GET", null, function (data) {
        model.suggestionRecommended.removeAll();
        model.suggestionRecommended.push.apply(model.suggestionRecommended, data);
        suggestionViewModel = Mapping(model.suggestionRecommended());//mapping to extend methods or properties
        model.latestRecommendedViewModel.push.apply(model.suggestionRecommendedViewModel, suggestionViewModel());

    }, null, { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' });
}
