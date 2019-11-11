import React from 'react';
import LinearProgress from "@material-ui/core/LinearProgress";

class ConfigLoader extends React.Component {
    componentDidMount() {
        this.props.onLoadConfig();
    }

    render() {
        return <LinearProgress />
    }
}

export default ConfigLoader;