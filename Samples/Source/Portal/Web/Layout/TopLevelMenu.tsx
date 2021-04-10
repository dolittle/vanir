import React from 'react';
import { useHistory } from 'react-router-dom';
import { PivotItem, Pivot } from '@fluentui/react';

export const TopLevelMenu = () => {
    const history = useHistory();

    function linkClicked(item?: PivotItem, ev?: React.MouseEvent<HTMLElement>) {
        history.push(item?.props.itemKey!);
    }
    return (
        <Pivot onLinkClick={linkClicked}>
            <PivotItem headerText="Home" itemIcon="Home" itemKey="/" />
            <PivotItem headerText="Menu" itemIcon="ProductCatalog" itemKey="/menu" />
        </Pivot>
    )
}
