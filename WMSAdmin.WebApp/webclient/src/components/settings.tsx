import { useTrackedGlobalState, useUpdateGlobalState } from '../utilities/store';
import { useEffect } from 'react';

export const Settings = () => {
    const updateAppConfig = useUpdateGlobalState();
    const appConfig = useTrackedGlobalState();
    var globalStrings = appConfig.GeneralString;
    let title = globalStrings!.settings;

    useEffect(() => {
        updateAppConfig((prev) => ({ ...prev, currentTitle: title }));
    }, [appConfig.currentTitle]);

    
    return (
        <div>
            <h1>{title}</h1>
        </div>
    );
}