"use strict";

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _config = require("./config");

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

var Rate = function (_React$Component) {
    _inherits(Rate, _React$Component);

    function Rate() {
        _classCallCheck(this, Rate);

        return _possibleConstructorReturn(this, (Rate.__proto__ || Object.getPrototypeOf(Rate)).apply(this, arguments));
    }

    _createClass(Rate, [{
        key: "render",
        value: function render() {
            var cur_float = parseFloat(this.props.cur_value);
            var prev_float = parseFloat(this.props.prev_value);

            // Rate stagnating
            var chr = "▬";
            var trend_type = "trend_stagnating";

            // Rate going up
            if (cur_float > prev_float) {
                chr = "▲";
                trend_type = "trend_growing";
            }
            // Rate going down
            else if (cur_float < prev_float) {
                    chr = "▼";
                    trend_type = "trend_declining";
                }

            return React.createElement(
                "tr",
                null,
                React.createElement(
                    "th",
                    null,
                    this.props.from,
                    "/",
                    this.props.to
                ),
                React.createElement(
                    "th",
                    null,
                    this.props.cur_value
                ),
                React.createElement(
                    "th",
                    null,
                    React.createElement(
                        "span",
                        { className: trend_type },
                        chr
                    )
                )
            );
        }
    }]);

    return Rate;
}(React.Component);

var RatesList = function (_React$Component2) {
    _inherits(RatesList, _React$Component2);

    function RatesList(props) {
        _classCallCheck(this, RatesList);

        // Binding methods so that we can call them from
        // inside other methods
        var _this2 = _possibleConstructorReturn(this, (RatesList.__proto__ || Object.getPrototypeOf(RatesList)).call(this, props));

        _this2.set_rates = _this2.set_rates.bind(_this2);
        _this2.update_rates = _this2.update_rates.bind(_this2);

        _this2.state = {
            pairs: null,
            rates: null,
            prev_rates: null
        };
        return _this2;
    }

    _createClass(RatesList, [{
        key: "set_pairs",
        value: function set_pairs(json) {
            // Update the state with the new pairs.
            this.setState({ pairs: json });

            // Once we have the pairs, make the first request for rates.
            this.update_rates();
        }
    }, {
        key: "set_rates",
        value: function set_rates(json) {
            // For some reason sometimes set_rates is called with empty json.
            if (jQuery.isEmptyObject(json)) {
                return;
            }

            console.log("Rates set!");

            // We want to check if some of the currency pairs suddenly
            // were removed from the server.
            var new_pairs = {};
            for (var id in json) {
                new_pairs[id] = this.state.pairs[id];
            }
            this.setState({ pairs: new_pairs });

            // If this is not the first update of rates
            if (this.state.prev_rates) {
                // We want to change previous rates only if a change in 
                // current rate appeared at the moment of the call (so that
                // if this method is called when the rates didn't update yet,
                // we don't set the prev_rates to a copy of current rates).
                var prev_rates = {};
                for (var _id in json) {
                    // If nothing changed on this iteration.
                    if (json[_id] == this.state.rates[_id]) {
                        prev_rates[_id] = this.state.prev_rates[_id];
                    }
                    // If a change was detected.
                    else {
                            prev_rates[_id] = this.state.rates[_id];
                        }
                }

                // Updating previous and current rates.
                this.setState({ prev_rates: prev_rates, rates: json });
            }
            // If this is the first update of rates
            else {
                    this.setState({ prev_rates: json, rates: json });
                }
        }
    }, {
        key: "update_rates",
        value: function update_rates() {
            var _this3 = this;

            var ids = [];
            for (var id in this.state.pairs) {
                ids.push(id);
            }

            $.ajax({
                method: "GET",
                url: _config.endpoint + "rates",
                dataType: "json",
                timeout: 500,
                data: { currencyPairIds: ids },
                success: function success(json) {
                    _this3.set_rates(json.rates);
                },
                error: function error(request, status, err) {
                    console.log("Error while requesting rates! Requesting again...");
                    setTimeout(_this3.update_rates, 500);
                }
            });
        }
    }, {
        key: "render",
        value: function render() {
            // If we didn't get the currency pairs yet:
            if (!this.state.pairs) {
                return React.createElement(
                    "tr",
                    null,
                    React.createElement(
                        "th",
                        null,
                        "Fetching currency pairs..."
                    )
                );
            }

            // Otherwise:
            else {
                    // Going through all currency pairs.
                    var pairs = [];
                    for (var id in this.state.pairs) {
                        // If the pair is not checked, skipping it.
                        if (!this.props.get_pair_state(id)) {
                            continue;
                        }

                        // Fetching the shortcut names.
                        var from = this.state.pairs[id][0].code;
                        var to = this.state.pairs[id][1].code;

                        // If we didn't get the rates yet, they will not be displayed.
                        var cur = "Updating...";
                        var prev = "Updating...";
                        if (this.state.rates && this.state.rates[id]) {
                            cur = this.state.rates[id];
                            prev = this.state.prev_rates[id];
                        }

                        // Making a new rate component with the needed values.
                        pairs.push(React.createElement(Rate, { key: id, from: from, to: to, cur_value: cur, prev_value: prev }));
                    }

                    return pairs;
                }
        }
    }]);

    return RatesList;
}(React.Component);

var RateSelector = function (_React$Component3) {
    _inherits(RateSelector, _React$Component3);

    function RateSelector() {
        _classCallCheck(this, RateSelector);

        return _possibleConstructorReturn(this, (RateSelector.__proto__ || Object.getPrototypeOf(RateSelector)).apply(this, arguments));
    }

    _createClass(RateSelector, [{
        key: "render",
        value: function render() {
            var class_name = this.props.checked ? "checked" : "unchecked";
            return React.createElement(
                "tr",
                { className: class_name },
                React.createElement(
                    "th",
                    null,
                    React.createElement(
                        "div",
                        { className: "checkbox" },
                        React.createElement("input", { type: "checkbox", name: this.props.name, id: this.props.id,
                            checked: this.props.checked, onChange: this.props.onChange }),
                        React.createElement(
                            "label",
                            { htmlFor: this.props.id },
                            " ",
                            this.props.name,
                            " "
                        )
                    )
                )
            );
        }
    }]);

    return RateSelector;
}(React.Component);

var RateSelectorList = function (_React$Component4) {
    _inherits(RateSelectorList, _React$Component4);

    function RateSelectorList(props) {
        _classCallCheck(this, RateSelectorList);

        var _this5 = _possibleConstructorReturn(this, (RateSelectorList.__proto__ || Object.getPrototypeOf(RateSelectorList)).call(this, props));

        _this5.state = {
            names: null,
            is_checked: new Map()
        };

        _this5.handleChange = _this5.handleChange.bind(_this5);
        _this5.get_state = _this5.get_state.bind(_this5);
        return _this5;
    }

    _createClass(RateSelectorList, [{
        key: "set_names",
        value: function set_names(json) {
            var names = {};
            var is_checked = new Map();

            // If the selection is stored in cookies, get it.
            var selection = null;
            if (Cookies.get('selection')) {
                selection = JSON.parse(Cookies.get('selection'));
            }

            for (var id in json) {
                names[id] = json[id][0].code + "/" + json[id][1].code;

                is_checked[id] = true;
                if (selection && !(typeof selection[id] === "undefined")) {
                    is_checked[id] = selection[id];
                }
            }

            this.setState({ names: names, is_checked: is_checked });
        }
    }, {
        key: "get_state",
        value: function get_state(id) {
            return this.state.is_checked[id];
        }
    }, {
        key: "handleChange",
        value: function handleChange(e) {
            var id = e.target.id;
            var new_checked = this.state.is_checked;

            new_checked[id] = !this.state.is_checked[id];
            this.setState({ is_checked: new_checked });

            // Update function passed from parent ExchangeRateClient class.
            this.props.redraw_rate_list();

            // Save current state to cookies.
            Cookies.set('selection', JSON.stringify(this.state.is_checked));
        }
    }, {
        key: "render",
        value: function render() {
            if (!this.state.names) {
                return React.createElement(
                    "tr",
                    null,
                    React.createElement(
                        "th",
                        null,
                        "Fetching currency pairs..."
                    )
                );
            }

            var checkboxes = [];
            for (var id in this.state.names) {
                var name = this.state.names[id];
                var checked = this.state.is_checked[id];
                checkboxes.push(React.createElement(RateSelector, { key: id, id: id, name: name,
                    checked: checked, onChange: this.handleChange }));
            }

            return checkboxes;
        }
    }]);

    return RateSelectorList;
}(React.Component);

var ExchangeRateClient = function (_React$Component5) {
    _inherits(ExchangeRateClient, _React$Component5);

    function ExchangeRateClient(props) {
        _classCallCheck(this, ExchangeRateClient);

        var _this6 = _possibleConstructorReturn(this, (ExchangeRateClient.__proto__ || Object.getPrototypeOf(ExchangeRateClient)).call(this, props));

        _this6.request_configuration = function () {
            console.log("Configuration request");

            // Loading previous configuration, if it is stored in cookies.
            if (Cookies.get('configuration')) {
                var json = JSON.parse(Cookies.get('configuration'));
                _this6.rate_list.current.set_pairs(json.currencyPairs);
                _this6.rate_selector_list.current.set_names(json.currencyPairs);
                console.log("Restored configuration from cookies.");
            }

            $.ajax({
                method: "GET",
                url: _config.endpoint + "configuration",
                dataType: "json",

                // Timeout is set to 4.5s in order to show the functionality 
                // of the error handler (the server gives timeouts in range
                // from 3s to 5s, so the error will be handled quite often).
                // I'm not sure if I really needed to implement it, though.
                timeout: 4500,
                success: function success(json) {
                    // If not all components are defined yet.
                    if (!_this6.rate_list || !_this6.rate_selector_list) {
                        console.log("Not all child components generated yet!");
                        setTimeout(_this6.request_configuration, 100);
                        return;
                    }

                    console.log("Currency pairs request success:");
                    console.log(json);

                    // Saving the received configuration to cookies.
                    Cookies.set('configuration', JSON.stringify(json));

                    // Updating pairs for rates list and repeatedly requesting rates.
                    _this6.rate_list.current.set_pairs(json.currencyPairs);
                    setInterval(_this6.rate_list.current.update_rates, _config.interval);

                    // Updating names for the pair selector, too.
                    _this6.rate_selector_list.current.set_names(json.currencyPairs);
                },
                error: function error(request, status, err) {
                    console.log("Error while requesting currency pairs! Requesting again...");
                    setTimeout(_this6.request_configuration, 500);
                }
            });
        };

        _this6.redraw_rate_list = function () {
            _this6.rate_list.current.forceUpdate();
        };

        _this6.get_pair_state = function (id) {
            // Is the given pair selected?
            if (!_this6.rate_selector_list.current) return true;
            return _this6.rate_selector_list.current.get_state(id);
        };

        _this6.rate_selector_list = React.createRef();
        _this6.rate_list = React.createRef();
        return _this6;
    }

    _createClass(ExchangeRateClient, [{
        key: "componentDidMount",
        value: function componentDidMount() {
            this.request_configuration();
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement(
                "div",
                null,
                React.createElement(
                    "div",
                    { id: "rates-selector" },
                    React.createElement(
                        "table",
                        { className: "selector-table" },
                        React.createElement(
                            "tbody",
                            null,
                            React.createElement(RateSelectorList, { ref: this.rate_selector_list,
                                redraw_rate_list: this.redraw_rate_list })
                        )
                    )
                ),
                React.createElement(
                    "div",
                    { id: "rates-list" },
                    React.createElement(
                        "table",
                        { className: "rates-table" },
                        React.createElement(
                            "tbody",
                            null,
                            React.createElement(RatesList, { ref: this.rate_list, get_pair_state: this.get_pair_state })
                        )
                    )
                )
            );
        }
    }]);

    return ExchangeRateClient;
}(React.Component);

ReactDOM.render(React.createElement(ExchangeRateClient, null), document.getElementById('exchange-rate-client'));