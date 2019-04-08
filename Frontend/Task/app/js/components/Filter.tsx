import * as React from 'react';
import Select from 'react-select';
import { CurrencyPairs } from '../interfaces/CurrencyPairs';
import { Option } from 'react-select/lib/filters';
import { State, updateFilter } from '../store/store';
import { connect } from 'react-redux';

const parseOptions = (pairs: CurrencyPairs): Option[] => {
	return Object.keys(pairs).map(key => {
		let pair = pairs[key];
		return {
			value: key,
			label: `${pair.pair[0].code}/${pair.pair[1].code}`,
			data: null
		}
	})
}

const Filter = ({ pairs, filteredIds, update }: { pairs: CurrencyPairs, filteredIds: string[], update: (ids: string[]) => void }) => {
	let options = parseOptions(pairs);
	let selectedOptions = options.filter(option => filteredIds.indexOf(option.value) > -1)
	return <Select options={options} value={selectedOptions} isMulti onChange={(val: Option[]) => update(val.map(opt => opt.value))} />
}

const mapStateToProps = (state: State) => {
	return {
		pairs: state.pairs,
		filteredIds: state.filteredIds || []
	};
}

const mapDispatchToProps = dispatch => {
	return {
	  update: (ids: string[]) => dispatch(updateFilter(ids)),
	}
}

export default connect(
	mapStateToProps,
	mapDispatchToProps
)(Filter);