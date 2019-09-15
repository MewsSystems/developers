import * as React from 'react';

interface ErrorBoundaryState {
    error: any,
    errorInfo: any
}

export class ErrorBoundary extends React.Component<{}, ErrorBoundaryState> {
    constructor(props: any) {
        super(props);
        this.state = { error: null, errorInfo: null };
    }

    componentDidCatch(error: any, info: any) {
        this.setState({
            error: error,
            errorInfo: info
        })
    }

    render() {
        if (this.state.errorInfo) {
            // Error path
            return (
                <section className="ErrorBoundary" style={{padding: "0 15px"}}>
                    <h2>Something went wrong.</h2>
                    <details style={{ whiteSpace: 'pre-wrap' }}>
                        {this.state.error && this.state.error.toString()}
                        <br />
                        {this.state.errorInfo.componentStack}
                    </details>
                </section>
            );
        }

        return this.props.children;
    }
}