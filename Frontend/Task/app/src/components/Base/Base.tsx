import React from 'react';
import styled from 'styled-components'
import * as skin from './Base.skin'
import {connect} from 'react-redux';
import {get_config} from '../../store/actions/currencyPairsRates.actions'

import Preloader from '../Preloader'
import CurrencyPairs from '../CurrencyPairs'

const PreloaderBase = styled(Preloader)`${skin.PreloaderBase}`;

type BaseProps = {
    get_config: Function,
    loading: boolean,
    error: boolean
}

class Base extends React.Component<BaseProps> {

    componentDidMount() {
        this.props.get_config();
    }

    render() {
        return (
            <div>
                {this.props.loading && (
                    <PreloaderBase/>
                )}
                <CurrencyPairs/>
            </div>
        );
    }
}

const mapStateToProps = state => {
    return {
        loading: state.configLoading,
        error: state.configError
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        get_config: () => dispatch(get_config())
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(Base);
