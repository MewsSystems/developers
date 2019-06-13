import React, { Component } from 'react';
import Button from './Button';

class CurrencyFilter extends Component {
  state = { visible: false };

  componentDidMount() {
    document.addEventListener('click', this.handleDocumentClick);
  };

  componentWillUnmount() {
    document.removeEventListener('click', this.handleDocumentClick);
  };

  handleDocumentClick = (e) => {
    if (e.target.contains(this.currencyFilter) && this.state.visible) {
      this.setState({ visible: false });
    }
  };

  onChange = (code) => {
    let { filter } = this.props;
    let index = filter.indexOf(code);
    index > -1 ? filter.splice(index, 1) : filter.push(code);
    this.props.updateFilter(filter);
  };

  selectAll = () => {
    let { filter, currencies } = this.props;
    this.props.updateFilter(filter.length === currencies.length ? [] : currencies.map(x => x.code));
  };

  render() {
    const { visible } = this.state;
    const { currencies, filter } = this.props;

    return (
      <div className="currency-filter" ref={el => this.currencyFilter = el}>
        <Button onClick={() => this.setState({ visible: !visible })}>Currencies <i className={`fas fa-chevron-${visible ? 'up' : 'down'}`} /></Button>
        {visible &&
          <div className="currency-dropdown">
            <ul>
              {currencies && currencies.length ?
                <>
                  <li onClick={this.selectAll} className={filter.length === currencies.length ? 'selected' : ''}>
                    {filter.length === currencies.length ? 'Deselect All' : 'Select All'}
                  </li>
                  {currencies.map(x => (
                    <li key={x.code} className={filter.indexOf(x.code) > -1 ? 'selected' : ''} onClick={() => this.onChange(x.code)}>
                      {x.code}
                    </li>
                  ))}
                </>
                :
                <li></li>
              }
            </ul>
          </div>
        }
      </div>
    );
  };
};

export default CurrencyFilter;