import { useEffect, useState } from 'react';
import { Config, fetchConfiguration } from './dataFetching/fetchConfiguration';

export function useConfig(configUrl: string): [Config | null, boolean] {
  const [config, setConfig] = useState<Config | null>(null);
  const [loadingFailed, setLoadingFailed] = useState(false);

  useEffect(() => {
    fetchConfiguration(configUrl)
      .then(setConfig)
      .catch(() => {
        setLoadingFailed(true);
      });
  }, [configUrl]);

  return [config, loadingFailed];
}
