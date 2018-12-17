var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _templateObject = _taggedTemplateLiteral(["", ""], ["", ""]);

function _taggedTemplateLiteral(strings, raw) { return Object.freeze(Object.defineProperties(strings, { raw: { value: Object.freeze(raw) } })); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

import styled, { css } from "styled-components";
import { endpoint, interval } from "../config.json";
import { TableStyles } from "../../css/component-styles.js";

import { RateList } from "./rate-list.js";
import { RateSelectorList } from "./selector-list.js";

var ExchangeRateClient = function (_React$Component) {
    _inherits(ExchangeRateClient, _React$Component);

    function ExchangeRateClient(props) {
        _classCallCheck(this, ExchangeRateClient);

        var _this = _possibleConstructorReturn(this, (ExchangeRateClient.__proto__ || Object.getPrototypeOf(ExchangeRateClient)).call(this, props));

        _this.requestConfiguration = function () {
            console.log("Configuration request");

            // Loading previous configuration, if it is stored in local storage or cookies.
            var prevConfig = null;
            if (typeof Storage !== "undefined" && localStorage.getItem("configuration")) {
                prevConfig = JSON.parse(localStorage.getItem("configuration"));
                console.log("Restored configuration from local storage.");
            } else if (Cookies.get("configuration")) {
                prevConfig = JSON.parse(Cookies.get("configuration"));
                console.log("Restored configuration from cookies.");
            }

            if (prevConfig) {
                _this.rateList.current.setPairs(prevConfig.currencyPairs);
                _this.rateSelectorList.current.setNames(prevConfig.currencyPairs);
            }

            // Requesting new configuration.
            axios.get(endpoint + "/configuration", {
                // Timeout is set to 4.5s in order to show the functionality 
                // of the error handler (the server gives timeouts in range
                // from 3s to 5s, so the error will be handled quite often).
                // I'm not sure if I really needed to implement it, though.
                timeout: 4500
            }).then(function (response) {
                // If not all components are defined yet.
                if (!_this.rateList || !_this.rateSelectorList) {
                    console.log("Not all child components generated yet!");
                    setTimeout(_this.requestConfiguration, 100);
                    return;
                }

                console.log("Currency pairs request success:");
                console.log(response.data);

                // Saving the received configuration to local storage or cookies.
                if (typeof Storage !== "undefined") {
                    localStorage.setItem("configuration", JSON.stringify(response.data));
                    console.log("Saved configuration to local storage.");
                } else {
                    Cookies.set("configuration", JSON.stringify(response.data));
                    console.log("Saved configuration to cookies.");
                }

                // Updating pairs for rates list and repeatedly requesting rates.
                _this.rateList.current.setPairs(response.data.currencyPairs);
                setInterval(_this.rateList.current.updateRates, interval);

                // Updating names for the pair selector, too.
                _this.rateSelectorList.current.setNames(response.data.currencyPairs);
            }).catch(function (error) {
                console.log("Error while requesting configuration: (" + error + ") Requesting again...");
                setTimeout(_this.requestConfiguration, 500);
            });
        };

        _this.redrawRateList = function () {
            _this.rateList.current.forceUpdate();
        };

        _this.isPairSelected = function (id) {
            if (!_this.rateSelectorList.current) return true;
            return _this.rateSelectorList.current.isSelected(id);
        };

        _this.rateSelectorList = React.createRef();
        _this.rateList = React.createRef();
        return _this;
    }

    _createClass(ExchangeRateClient, [{
        key: "componentDidMount",
        value: function componentDidMount() {
            this.requestConfiguration();
        }
    }, {
        key: "render",
        value: function render() {
            var Table = styled.table(_templateObject, TableStyles);

            return React.createElement(
                "div",
                null,
                React.createElement(
                    "div",
                    { className: "rateSelector" },
                    React.createElement(
                        Table,
                        null,
                        React.createElement(
                            "tbody",
                            null,
                            React.createElement(RateSelectorList, { id: this.props.id, ref: this.rateSelectorList,
                                redrawRateList: this.redrawRateList })
                        )
                    )
                ),
                React.createElement(
                    "div",
                    { className: "rateList" },
                    React.createElement(
                        Table,
                        null,
                        React.createElement(
                            "tbody",
                            null,
                            React.createElement(RateList, { id: this.props.id, ref: this.rateList,
                                isPairSelected: this.isPairSelected })
                        )
                    )
                )
            );
        }
    }]);

    return ExchangeRateClient;
}(React.Component);

ReactDOM.render(React.createElement(ExchangeRateClient, { id: "client-1" }), document.getElementById("exchange-rate-client"));