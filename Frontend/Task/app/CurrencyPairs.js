"use strict";

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

//import { endpoint, interval } from './config';

var CurrencyPairs = function (_React$Component) {
  _inherits(CurrencyPairs, _React$Component);

  function CurrencyPairs(props) {
    _classCallCheck(this, CurrencyPairs);

    var _this = _possibleConstructorReturn(this, (CurrencyPairs.__proto__ || Object.getPrototypeOf(CurrencyPairs)).call(this, props));

    _this.state = {
      error: null,
      isLoaded: false,
      items: []
    };

    _this.currencyPairs = [];
    _this.endpoint = props.endpoint.replace("/rates", "");
    return _this;
  }

  _createClass(CurrencyPairs, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var _this2 = this;

      var me = this;
      fetch(me.endpoint + "/configuration").then(function (res) {
        return res.json();
      }).then(function (result) {
        for (var item in result.currencyPairs) {
          me.currencyPairs.push({ id: item, pairs: result.currencyPairs[item] });
        }
        _this2.setState({
          isLoaded: true,
          items: me.currencyPairs
        });
      },
      // Note: it's important to handle errors here
      // instead of a catch() block so that we don't swallow
      // exceptions from actual bugs in components.
      function (error) {
        _this2.setState({
          isLoaded: true,
          error: error
        });
      });
    }
  }, {
    key: "render",
    value: function render() {
      var _state = this.state,
          error = _state.error,
          isLoaded = _state.isLoaded,
          items = _state.items;

      if (error) {
        return React.createElement(
          "div",
          null,
          "Error: ",
          error.message
        );
      } else if (!isLoaded) {
        return React.createElement(
          "div",
          null,
          "Loading..."
        );
      } else {
        return React.createElement(
          "ul",
          null,
          items.map(function (item) {
            return React.createElement(
              "li",
              { key: item.id },
              item.pairs[0].code,
              "/",
              item.pairs[1].code
            );
          })
        );
      }
    }
  }]);

  return CurrencyPairs;
}(React.Component);