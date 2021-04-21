import React from 'react';
import { Search } from '../Search';
import { BrowserRouter as Router, Route, Switch, useHistory } from 'react-router-dom';
import { TopLevelMenu } from './TopLevelMenu';
import { Home } from '../Home';
import { SearchResult } from '../Search/SearchResult';

import { default as styles } from './Layout.module.scss';
import { Blah } from '../Blah';
import { CompositionRoute } from '@dolittle/vanir-react';

export const Layout = () => {
    return (
        <div>
            <div style={{ backgroundColor: '#333' }}>
                <TopLevelMenu />
            </div>
            <div style={{ backgroundColor: '#555' }}>
                <Search />
            </div>

            <div className={styles.content}>
                <Switch>
                    <Route exact path="/">
                        <Home />
                    </Route>
                    <Route path="/search">
                        <SearchResult />
                    </Route>
                    <Route path="/blah">
                        <Blah />
                    </Route>
                    <CompositionRoute path="/typescript" />
                    <CompositionRoute path="/restaurant" />
                    <CompositionRoute path="/kitchen" />
                </Switch>
            </div>
        </div>
    );
}
