import React, { Component } from 'react';
// redux
import { connect } from 'react-redux';
import { setTogglePairs } from '../store/actions';


class CurrencySelector extends Component {
  constructor(props) {
    super(props);

    this.state = {
      displayFilters: false,
      togglePairs: {}
    }

    this.displayFilters = this.displayFilters.bind(this);
    this.handleDisplayPair = this.handleDisplayPair.bind(this);
  }

  displayFilters() {
    this.setState((prevState) => ({
      togglePairs: JSON.parse(localStorage.getItem('togglePairs')),
      displayFilters: !prevState.displayFilters
    }));
  }

  handleDisplayPair(pair) {
    this.setState((prevState) => ({ 
      togglePairs: {
        ...prevState.togglePairs,
        [pair]: !prevState.togglePairs[pair]
      }
    }))
  }

  render() {
    const { currencyPairs } = this.props;
    const { displayFilters, togglePairs } = this.state;

    const filterWrap = {
      display: 'inline-block',
      marginBottom: '10px',
      fontFamily: 'Verdana', fontSize: '10px'
    };
    const filterBtn = {
      backgroundColor: '#ddd',
      padding: '10px',
      display: 'inline-block'
    };
    const filters = { 
      backgroundColor: '#f3f3f3' 
    };
    const filter = { 
      padding: '4px 8px' 
    }
    const filterToggler = {
      paddingLeft: '10px', 
      float: 'right'
    }

    return (
      <div style={filterWrap}>
        <a style={filterBtn} onClick={() => this.displayFilters()}>{'Filter Currency Pairs'}</a>
        {displayFilters &&
          <div style={filters}>
            {
              Object.keys(currencyPairs).map(pair => (
                <div style={filter} key={pair} onClick={() => this.handleDisplayPair(pair)}>
                  <span>{`${currencyPairs[pair][0].name} / ${currencyPairs[pair][1].name}`}</span>
                  <span style={filterToggler}>{(togglePairs[pair] ? 'hide' : 'display')}</span>
                </div>
              ))
            }
          </div>
        }
      </div>
    )
  }

  componentWillUpdate(nextProps, nextState) {
    this.props.setTogglePairs(nextState.togglePairs)
    localStorage.setItem('togglePairs', JSON.stringify(nextState.togglePairs))
  }
}

const mapStateToProps = state => ({ 
  currencyPairs: state.currencyPairs,
  togglePairs: state.togglePairs
});

const mapDispatchToProps = dispatch => ({
  setTogglePairs: togglePairs => dispatch(setTogglePairs(togglePairs))
});

export default connect(mapStateToProps, mapDispatchToProps)(CurrencySelector)