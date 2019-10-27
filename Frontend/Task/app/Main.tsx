import * as React from 'react';
import { useEffect, useState } from 'react';
import { Config, fetchConfiguration } from './dataFetching/fetchConfiguration';

type Props = {
  configUrl: string;
};

export const Main = ({ configUrl }: Props) => {
  const [config, setConfig] = useState<Config | null>(null);
  const [loadingFailed, setLoadingFailed] = useState(false);
  useEffect(() => {
    fetchConfiguration(configUrl)
      .then(setConfig)
      .catch(() => {
        setLoadingFailed(true);
      });
  }, [configUrl]);

  if (config) {
    return <div>Config loaded</div>;
  }
  if (loadingFailed) {
    return <div>Config loading failed</div>;
  }
  return <div>Loading config...</div>;
};
