import React from "react";
import { Spinner } from "react-activity";
import "react-activity/dist/react-activity.css";
import { connect } from "react-redux";
import { getConfiguration } from "./actions/index";
import { changeFilter } from "./actions/filter";
import DropdownFilter from "./components/DropownFilter";
import TableTitle from "./components/TableTitle";
import RatesTable from "./components/RatesTable";

class App extends React.Component {
  state = {
    filterIsVisible: false
  };

  componentDidMount = () => {
    this.props.getConfiguration();
  };

  changeFilter = async id => {
    await this.setState({ filterIsVisible: false });
    this.props.changeFilter(id, "select");
  };

  render() {
    return (
      <div className="App1">
        <div>
          <TableTitle
            filterIsVisible={this.state.filterIsVisible}
            showFilter={() =>
              this.setState({ filterIsVisible: !this.state.filterIsVisible })
            }
          />
          <div
            className="dropdown-filter"
            style={{ display: this.state.filterIsVisible ? "block" : "none" }}
          >
            <DropdownFilter {...this.props} />
          </div>
        </div>
        <RatesTable changeFilter={this.changeFilter} data={this.props.data} />

        <div className="loading">{this.props.status && <Spinner />}</div>
      </div>
    );
  }
}

const mapStateToProps = state => ({
  config: state.config,
  status: state.status,
  filter: state.filter,
  data: state.data
});

const mapDispatchToProps = dispatch => ({
  getConfiguration: () => {
    dispatch(getConfiguration());
  },
  changeFilter: (key, type) => {
    dispatch(changeFilter(key, type));
  }
});

export default connect(mapStateToProps, mapDispatchToProps)(App);
