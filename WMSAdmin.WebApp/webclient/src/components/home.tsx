import { useTrackedGlobalState, useUpdateGlobalState } from '../utilities/store';
import { useEffect } from 'react';
import { Utility } from '../utilities/utility';

export const Home = () => {
    const updateAppConfig = useUpdateGlobalState();
    const appConfig = useTrackedGlobalState();
    let title = appConfig.generalString!.home;
    
    useEffect(() => {
        updateAppConfig((prev) => ({ ...prev, currentTitle: title }));
    }, [appConfig.currentTitle]);

    return (
        <div>
            <h1>{title}</h1>
        </div>
    );
}