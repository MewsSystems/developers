var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _templateObject = _taggedTemplateLiteral(["", ""], ["", ""]);

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

function _taggedTemplateLiteral(strings, raw) { return Object.freeze(Object.defineProperties(strings, { raw: { value: Object.freeze(raw) } })); }

import styled, { css } from "styled-components";
import { ThStyles, TrStyles } from "../../css/component-styles.js";
import { endpoint } from "../config.json";

var Th = styled.th(_templateObject, ThStyles);
var Tr = styled.tr(_templateObject, TrStyles);

var RateEntry = function (_React$Component) {
    _inherits(RateEntry, _React$Component);

    function RateEntry() {
        _classCallCheck(this, RateEntry);

        return _possibleConstructorReturn(this, (RateEntry.__proto__ || Object.getPrototypeOf(RateEntry)).apply(this, arguments));
    }

    _createClass(RateEntry, [{
        key: "render",
        value: function render() {
            var curValueFloat = parseFloat(this.props.curValue);
            var prevValueFloat = parseFloat(this.props.prevValue);

            // Rate stagnating
            var chr = "▬";
            var trendType = "trendStagnating";

            // Rate going up
            if (curValueFloat > prevValueFloat) {
                chr = "▲";
                trendType = "trendGrowing";
            }
            // Rate going down
            else if (curValueFloat < prevValueFloat) {
                    chr = "▼";
                    trendType = "trendDeclining";
                }

            return React.createElement(
                Tr,
                { className: "RateList" },
                React.createElement(
                    Th,
                    { className: "RateList" },
                    this.props.from,
                    "/",
                    this.props.to
                ),
                React.createElement(
                    Th,
                    { className: "RateList" },
                    this.props.curValue
                ),
                React.createElement(
                    Th,
                    { className: "RateList" },
                    React.createElement(
                        "span",
                        { className: trendType },
                        chr
                    )
                )
            );
        }
    }]);

    return RateEntry;
}(React.Component);

export var RateList = function (_React$Component2) {
    _inherits(RateList, _React$Component2);

    function RateList(props) {
        _classCallCheck(this, RateList);

        // Binding methods so that we can call them from
        // inside other methods.
        var _this2 = _possibleConstructorReturn(this, (RateList.__proto__ || Object.getPrototypeOf(RateList)).call(this, props));

        _this2.setRates = _this2.setRates.bind(_this2);
        _this2.updateRates = _this2.updateRates.bind(_this2);

        _this2.state = Object.freeze({
            pairs: null,
            rates: null,
            prevRates: null
        });
        return _this2;
    }

    _createClass(RateList, [{
        key: "setPairs",
        value: function setPairs(newPairs) {
            // Update the state with the new pairs.
            this.setState({ pairs: newPairs });

            // Once we have the pairs, make the first request for rates.
            this.updateRates();
        }
    }, {
        key: "setRates",
        value: function setRates(newRates) {
            // For some reason sometimes setRates is called with empty json.
            if (Object.getOwnPropertyNames(newRates).length === 0) {
                return;
            }

            // We want to check if some of the currency pairs suddenly
            // were removed from the server.
            var newPairs = {};
            for (var id in newRates) {
                newPairs[id] = this.state.pairs[id];
            }
            this.setState({ pairs: newPairs });

            // If this is not the first update of rates
            if (this.state.prevRates) {
                // We want to change previous rates only if a change in 
                // current rate appeared at the moment of the call (so that
                // if this method is called when the rates didn't update yet,
                // we don't set the prevRates to a copy of current rates).
                var prevRatesUpdated = {};
                for (var _id in newRates) {
                    // If nothing changed on this iteration.
                    if (newRates[_id] == this.state.rates[_id]) {
                        prevRatesUpdated[_id] = this.state.prevRates[_id];
                    }
                    // If a change was detected.
                    else {
                            prevRatesUpdated[_id] = this.state.rates[_id];
                        }
                }

                // Updating previous and current rates.
                this.setState({ prevRates: prevRatesUpdated, rates: newRates });
            }
            // If this is the first update of rates
            else {
                    this.setState({ prevRates: newRates, rates: newRates });
                }
        }
    }, {
        key: "updateRates",
        value: function updateRates() {
            var _this3 = this;

            var ids = [];
            for (var id in this.state.pairs) {
                ids.push(id);
            }

            axios.get(endpoint + "/rates", {
                timeout: 500,
                params: {
                    currencyPairIds: ids
                }
            }).then(function (response) {
                _this3.setRates(response.data.rates);
            }).catch(function (error) {
                console.log("Error while requesting rates: (" + error + ") Requesting again...");
                setTimeout(_this3.updateRates, 500);
            });
        }
    }, {
        key: "render",
        value: function render() {
            // If we didn't get the currency pairs yet:
            if (!this.state.pairs) {
                return null;
            }

            // Otherwise:
            else {
                    // Going through all currency pairs.
                    var pairs = [];
                    for (var id in this.state.pairs) {
                        // If the pair is not checked, skipping it.
                        if (!this.props.isPairSelected(id)) {
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
                            prev = this.state.prevRates[id];
                        }

                        // Making a new rate component with the needed values.
                        pairs.push(React.createElement(RateEntry, { key: id, from: from, to: to, curValue: cur, prevValue: prev }));
                    }

                    return pairs;
                }
        }
    }]);

    return RateList;
}(React.Component);