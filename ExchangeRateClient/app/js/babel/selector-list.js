var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _templateObject = _taggedTemplateLiteral(["", ""], ["", ""]);

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

function _taggedTemplateLiteral(strings, raw) { return Object.freeze(Object.defineProperties(strings, { raw: { value: Object.freeze(raw) } })); }

import styled, { css } from "styled-components";
import { ThStyles, TrStyles } from "../../css/component-styles.js";

var Th = styled.th(_templateObject, ThStyles);
var Tr = styled.tr(_templateObject, TrStyles);

var SelectorEntry = function (_React$Component) {
    _inherits(SelectorEntry, _React$Component);

    function SelectorEntry() {
        _classCallCheck(this, SelectorEntry);

        return _possibleConstructorReturn(this, (SelectorEntry.__proto__ || Object.getPrototypeOf(SelectorEntry)).apply(this, arguments));
    }

    _createClass(SelectorEntry, [{
        key: "render",
        value: function render() {
            var type = this.props.checked ? "checked" : "unchecked";

            return React.createElement(
                Tr,
                { className: "SelectorList", type: type },
                React.createElement(
                    Th,
                    { className: "SelectorList" },
                    React.createElement(
                        "div",
                        { className: "checkbox" },
                        React.createElement("input", { type: "checkbox", name: this.props.name, id: this.props.id,
                            pair_id: this.props.pair_id, checked: this.props.checked,
                            onChange: this.props.onChange }),
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

    return SelectorEntry;
}(React.Component);

export var RateSelectorList = function (_React$Component2) {
    _inherits(RateSelectorList, _React$Component2);

    function RateSelectorList(props) {
        _classCallCheck(this, RateSelectorList);

        var _this2 = _possibleConstructorReturn(this, (RateSelectorList.__proto__ || Object.getPrototypeOf(RateSelectorList)).call(this, props));

        _this2.state = Object.freeze({
            names: null,
            isChecked: new Map()
        });

        // Binding methods so that we can call them from
        // inside other methods.
        _this2.handleChange = _this2.handleChange.bind(_this2);
        _this2.isSelected = _this2.isSelected.bind(_this2);
        return _this2;
    }

    _createClass(RateSelectorList, [{
        key: "setNames",
        value: function setNames(newPairs) {
            var names = {};
            var isChecked = new Map();

            // If the selection is stored locally, get it.
            var selection = null;
            var storageName = "selection-" + this.props.id;
            if (typeof Storage !== "undefined" && localStorage.getItem(storageName)) {
                selection = JSON.parse(localStorage.getItem(storageName));
            } else if (Cookies.get(storageName)) {
                selection = JSON.parse(Cookies.get(storageName));
            }

            for (var id in newPairs) {
                names[id] = newPairs[id][0].code + "/" + newPairs[id][1].code;

                isChecked[id] = true;
                if (selection && typeof selection[id] !== "undefined") {
                    isChecked[id] = selection[id];
                }
            }

            this.setState({ names: names, isChecked: isChecked });
        }
    }, {
        key: "isSelected",
        value: function isSelected(id) {
            return this.state.isChecked[id];
        }
    }, {
        key: "handleChange",
        value: function handleChange(e) {
            var id = e.target.getAttribute("pair_id");
            var newChecked = new Map();

            for (var idIter in this.state.isChecked) {
                newChecked[idIter] = this.state.isChecked[idIter];
            }

            newChecked[id] = !this.state.isChecked[id];
            this.setState({ isChecked: newChecked });

            // Update function passed from parent ExchangeRateClient class.
            this.props.redrawRateList();

            // Save current state to local storage or cookies.
            var storageName = "selection-" + this.props.id;
            if (typeof Storage !== "undefined") {
                localStorage.setItem(storageName, JSON.stringify(newChecked));
                console.log("Saved selection to local storage.");
            } else {
                Cookies.set(storageName, JSON.stringify(newChecked));
                console.log("Saved selection to cookies.");
            }
        }
    }, {
        key: "render",
        value: function render() {
            if (!this.state.names) {
                return React.createElement(
                    Tr,
                    null,
                    React.createElement(
                        Th,
                        null,
                        "Please wait, fetching currency pairs..."
                    )
                );
            }

            var checkboxes = [];
            for (var pairId in this.state.names) {
                var name = this.state.names[pairId];
                var checked = this.state.isChecked[pairId];
                var checkboxId = this.props.id + "-" + pairId;

                checkboxes.push(React.createElement(SelectorEntry, { key: checkboxId, id: checkboxId, name: name,
                    pair_id: pairId, checked: checked,
                    onChange: this.handleChange }));
            }

            return checkboxes;
        }
    }]);

    return RateSelectorList;
}(React.Component);