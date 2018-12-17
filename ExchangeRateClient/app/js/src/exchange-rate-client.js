import styled, { css } from "styled-components";
import { endpoint, interval }  from "../config.json";
import { TableStyles } from "../../css/component-styles.js";

import { RateList } from "./rate-list.js";
import { RateSelectorList } from "./selector-list.js";


class ExchangeRateClient extends React.Component {
    constructor(props) {
        super(props);

        this.rateSelectorList = React.createRef();
        this.rateList         = React.createRef();
    }

    componentDidMount() {
        this.requestConfiguration();
    }

    requestConfiguration = () => {
        console.log("Configuration request");
        
        // Loading previous configuration, if it is stored in local storage or cookies.
        let prevConfig = null;
        if (typeof(Storage) !== "undefined" && localStorage.getItem("configuration")) {
            prevConfig = JSON.parse(localStorage.getItem("configuration"));
            console.log("Restored configuration from local storage.");
        } 
        else if (Cookies.get("configuration")) {
            prevConfig = JSON.parse(Cookies.get("configuration"));
            console.log("Restored configuration from cookies.");
        }

        if (prevConfig) {
            this.rateList.current.setPairs(prevConfig.currencyPairs);
            this.rateSelectorList.current.setNames(prevConfig.currencyPairs);
        }

        // Requesting new configuration.
        axios.get(`${endpoint}/configuration`, {
                // Timeout is set to 4.5s in order to show the functionality 
                // of the error handler (the server gives timeouts in range
                // from 3s to 5s, so the error will be handled quite often).
                // I'm not sure if I really needed to implement it, though.
                timeout: 4500,
            })
            .then((response) => {
                // If not all components are defined yet.
                if (!this.rateList || !this.rateSelectorList) {
                    console.log("Not all child components generated yet!");
                    setTimeout(this.requestConfiguration, 100);
                    return;
                }

                console.log("Currency pairs request success:");
                console.log(response.data);

                // Saving the received configuration to local storage or cookies.
                if (typeof(Storage) !== "undefined") {
                    localStorage.setItem("configuration", JSON.stringify(response.data));
                    console.log("Saved configuration to local storage.");
                } 
                else {
                    Cookies.set("configuration", JSON.stringify(response.data));
                    console.log("Saved configuration to cookies.");
                }

                // Updating pairs for rates list and repeatedly requesting rates.
                this.rateList.current.setPairs(response.data.currencyPairs);
                setInterval(this.rateList.current.updateRates, interval);

                // Updating names for the pair selector, too.
                this.rateSelectorList.current.setNames(response.data.currencyPairs);
            })
            .catch((error) => {
                console.log(`Error while requesting configuration: (${error}) Requesting again...`);
                setTimeout(this.requestConfiguration, 500);
            });
    }

    redrawRateList = () => {
        this.rateList.current.forceUpdate();
    }

    isPairSelected = (id) => {
        if (!this.rateSelectorList.current) return true;
        return this.rateSelectorList.current.isSelected(id);
    }

    render() {
        const Table = styled.table`${TableStyles}`;

        return (
            <div>
                <div className="rateSelector">
                    <Table>
                        <tbody>
                            <RateSelectorList id={this.props.id} ref={this.rateSelectorList} 
                                              redrawRateList={this.redrawRateList}/>
                        </tbody>
                    </Table>
                </div>
                <div className="rateList">
                    <Table>
                        <tbody>
                            <RateList id={this.props.id} ref={this.rateList}
                                      isPairSelected={this.isPairSelected}/>
                        </tbody>
                    </Table>
                </div>
            </div>
        );
    }
}

ReactDOM.render(
    <ExchangeRateClient id="client-1" />,
    document.getElementById("exchange-rate-client")
);
