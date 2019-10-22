import React, {useEffect, useCallback, useState} from 'react';
import { useDispatch, useSelector } from 'react-redux'
import {differenceBy} from 'lodash'

import * as actions from '../store/actions/index'
import axios from '../axios'
import withErrorHandler from '../hoc/withErrorHandler/withErrorHandler'

import {makeStyles} from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';

import {PairsList} from '../components/PairsList/PairsList'
import {CustomButton} from '../components/UI/Button/Button'
import Spinner from '../components/UI/Spinner/Spinner'

const useStyles = makeStyles(theme => ({
    root: {
        margin: 'auto',
    },
    button: {
        margin: theme.spacing(0.5, 0),
    },
}));

function intersection(a, b) {
    return a.filter(value => b.id !== value)
}

export const MainContent = () => {

    const classes = useStyles();

    const handleToggle = value => () => {
        const currentIndex = checked.indexOf(value);
        const newChecked = [...checked];

        if (currentIndex === -1) {
            newChecked.push(value);
        } else {
            newChecked.splice(currentIndex, 1);
        }
        setChecked(newChecked);
    };

    const handleAllRight = () => {
        onUpdatePairs([])
        onUpdateRates(rates.concat(left))
        setChecked([]);
    };

    const handleCheckedRight = () => {
        let updatedPairs = [];

        leftChecked.forEach(elem => {
            left.map(pair => {
                if (pair.id === elem) {
                    updatedPairs.push(pair)
                } else {
                    return null
                }
            })
        })

        let newLeft = differenceBy(left, updatedPairs, 'id')
        let newRates = rates.concat(updatedPairs)

        onUpdatePairs(newLeft)
        onUpdateRates(newRates)
        setChecked([])
    };

    const handleCheckedLeft = () => {
        let updatedPairs = [];

        rightChecked.forEach(elem => {
            right.map(pair => {
                if (pair.id === elem) {
                    updatedPairs.push(pair)
                }
            })
        })

        let newRates = differenceBy(right, updatedPairs, 'id')
        let newLeft = pairs.concat(updatedPairs)

        onUpdatePairs(newLeft)
        onUpdateRates(newRates)
        setChecked([])
    };

    const handleAllLeft = () => {
        onUpdatePairs(pairs.concat(right))
        onUpdateRates([])
        setChecked([])
    };

    const dispatch = useDispatch()
    const onInitPairs = useCallback(() => dispatch(actions.fetchPairs()), [dispatch])
    const onInitRates = useCallback((pairsLinks) => dispatch(actions.getRates(pairsLinks)), [dispatch])
    const onUpdatePairs = (pairs) => dispatch(actions.updatePairs(pairs))
    const onUpdateRates = useCallback((rates) => dispatch(actions.updateRates(rates)), [dispatch])

    const pairs = useSelector(state => {
        return state.pairs.pairs
    })

    const rates = useSelector(state => {
        return state.rates.rates
    })

    const allRates = useSelector(state => {
        return state.rates.allRates
    })

    const errorPairs = useSelector(state => {
        return state.pairs.error
    })

    const errorRates = useSelector(state => {
        return state.rates.error
    })

    const loadingPairs = useSelector(state => {
        return state.pairs.loading
    })

    const loadingRates = useSelector(state => {
        return state.rates.loading
    })

    const pairsLinks = useSelector(state => {
        return state.pairs.pairsLinks
    })


    useEffect(() => {
        onInitPairs()
    }, [onInitPairs])

    useEffect(() => {
        setLeft(pairs)
    }, [pairs])

    useEffect(() => {
        setRight(rates)
    }, [rates])


    useEffect(() => {
        onUpdateRates(rates)
    }, [onUpdateRates])

    const [checked, setChecked] = useState([]);
    const [left, setLeft] = useState(pairs);
    const [right, setRight] = useState(rates);

    const leftChecked = intersection(checked, left);
    const rightChecked = intersection(checked, right);

    let display = <></>

    if (pairs.length > 0) {
        display = (
            <Grid container spacing={2} justify="center" alignItems="center" className={classes.root}>
                <PairsList handleToggle={handleToggle} items={left} checked={checked} type='left' error={errorPairs}/>
                <Grid item>
                    <Grid container direction="column" alignItems="center">
                        <CustomButton
                            variant="outlined"
                            size="small"
                            classes={classes.button}
                            onclick={handleAllRight}
                            disabled={left.length === 0}
                            ariaLabel="move all right"
                            img="≫"
                        />
                        <CustomButton
                            variant="outlined"
                            size="small"
                            classes={classes.button}
                            onclick={handleCheckedRight}
                            disabled={leftChecked.length === 0}
                            ariaLabel="move selected right"
                            img="&gt;"
                        />
                        <CustomButton
                            variant="outlined"
                            size="small"
                            classes={classes.button}
                            onclick={handleCheckedLeft}
                            disabled={rightChecked.length === 0}
                            ariaLabel="move selected left"
                            img="&lt;"
                        />
                        <CustomButton
                            variant="outlined"
                            size="small"
                            classes={classes.button}
                            onclick={handleAllLeft}
                            disabled={right.length === 0}
                            ariaLabel="move all left"
                            img="≪"
                        />
                    </Grid>
                </Grid>
                <PairsList handleToggle={handleToggle} items={right} checked={checked} type='right' allRates={allRates} error={errorRates}/>
            </Grid>
        )
    } else if (loadingPairs || loadingRates){
        display = (<Spinner />)
    }

    return display
}

export default withErrorHandler(MainContent, axios)
