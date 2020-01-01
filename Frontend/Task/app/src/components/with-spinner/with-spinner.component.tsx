import React from 'react'
import Spinner from 'react-spinkit'
import './styles.module.css'

const WithSpinnerBody = WrappedComponent => ({isLoading, ...otherProps}) => {
    return isLoading ?
        (<tbody className="spinner-tbody">
            <Spinner className="spinner" name="ball-spin-fade-loader"/>
        </tbody>) :
        (<WrappedComponent {...otherProps} />);
}

const WithSpinnerRate = WrappedComponent => ({isLoading, ...otherProps}) => {
    return isLoading ?
        (<>
            <td className="spinner-value" scope="col"><Spinner className="spinner" name="circle"/></td>
            <td className="spinner-trend" scope="col"><Spinner className="spinner" name="circle"/></td>
        </>
        ) :
        (<WrappedComponent {...otherProps} />);

}


export {WithSpinnerBody, WithSpinnerRate}