import * as React from "react";

export interface WithUpdaterProps {
  rotationPaused?: boolean;
  onNextUpdate: () => void;
  interval: number;
}

export function WithUpdater(WrappedComponent) {
  class Updater extends React.Component<WithUpdaterProps> {
    static defaultProps = {
      rotationPaused: false
    };

    updaterInterval;

    componentDidMount() {
      this.startUpdating();
    }

    startUpdating = () => {
      if (this.props.rotationPaused) return;
      if (this.updaterInterval) return;

      this.updaterInterval = setInterval(
        this.props.onNextUpdate,
        this.props.interval
      );
    };

    pauseUpdating = () => {
      clearInterval(this.updaterInterval);
      this.updaterInterval = null;
    };

    componentDidUpdate() {
      if (this.props.rotationPaused) {
        this.pauseUpdating();
        return;
      }

      this.startUpdating();
    }

    componentWillUnmount() {
      const { updaterInterval: updaterInterval1 } = this;
      clearInterval(updaterInterval1);
    }

    render() {
      return <WrappedComponent {...this.props} />;
    }
  }

  return Updater;
}
