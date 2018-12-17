import styled, { css } from "styled-components";
import { ThStyles, TrStyles } from "../../css/component-styles.js";


const Th = styled.th`${ThStyles}`;
const Tr = styled.tr`${TrStyles}`;

class SelectorEntry extends React.Component {
    render() {
        let type = this.props.checked ? "checked" : "unchecked";

        return (
            <Tr className="SelectorList" type={type}>
                <Th className="SelectorList">
                    <div className="checkbox">
                        <input type="checkbox" name={this.props.name} id={this.props.id} 
                               pair_id={this.props.pair_id} checked={this.props.checked} 
                               onChange={this.props.onChange} />
                        <label htmlFor={this.props.id}> {this.props.name} </label>
                    </div>
                </Th>
            </Tr>
        );
    }
}

export class RateSelectorList extends React.Component {
    constructor(props) { 
        super(props); 
        this.state = Object.freeze({
            names: null,
            isChecked: new Map()
        });

        // Binding methods so that we can call them from
        // inside other methods.
        this.handleChange = this.handleChange.bind(this);
        this.isSelected   = this.isSelected.bind(this);
    }

    setNames(newPairs) {
        let names = {};
        let isChecked = new Map();

        // If the selection is stored locally, get it.
        let selection = null;
        let storageName = `selection-${this.props.id}`;
        if (typeof(Storage) !== "undefined" && localStorage.getItem(storageName)) {
            selection = JSON.parse(localStorage.getItem(storageName));
        }
        else if (Cookies.get(storageName)) {
            selection = JSON.parse(Cookies.get(storageName));
        }

        for (let id in newPairs) {
            names[id] = `${newPairs[id][0].code}/${newPairs[id][1].code}`;

            isChecked[id] = true;
            if (selection && typeof(selection[id]) !== "undefined") {
                isChecked[id] = selection[id];
            }
        }

        this.setState({names: names, isChecked: isChecked});
    }

    isSelected(id) {
        return this.state.isChecked[id];
    }

    handleChange(e) {
        const id = e.target.getAttribute("pair_id");
        let newChecked = new Map();

        for (let idIter in this.state.isChecked) {
            newChecked[idIter] = this.state.isChecked[idIter];
        }

        newChecked[id] = !this.state.isChecked[id];
        this.setState({isChecked: newChecked});

        // Update function passed from parent ExchangeRateClient class.
        this.props.redrawRateList();

        // Save current state to local storage or cookies.
        let storageName = `selection-${this.props.id}`;
        if (typeof(Storage) !== "undefined") {
            localStorage.setItem(storageName, JSON.stringify(newChecked));
            console.log("Saved selection to local storage.");
        }
        else {
            Cookies.set(storageName, JSON.stringify(newChecked));
            console.log("Saved selection to cookies.");
        }
    }

    render() {
        if (!this.state.names) {
            return (<Tr><Th>Please wait, fetching currency pairs...</Th></Tr>);
        }

        let checkboxes = [];
        for (let pairId in this.state.names) {
            let name = this.state.names[pairId];
            let checked = this.state.isChecked[pairId];
            let checkboxId = `${this.props.id}-${pairId}`;

            checkboxes.push(<SelectorEntry key={checkboxId} id={checkboxId} name={name} 
                                           pair_id={pairId} checked={checked} 
                                           onChange={this.handleChange} />);
        }

        return checkboxes;
    }
}
