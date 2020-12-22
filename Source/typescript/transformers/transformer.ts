// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


import * as ts from 'typescript';

export interface MyPluginOptions {
    some?: string
}

export default function myTransformerPlugin(program: ts.Program, opts: MyPluginOptions) {
    return {
        before(ctx: ts.TransformationContext) {
            return (sourceFile: ts.SourceFile) => {
                function visitor(node: ts.Node): ts.Node {
                    if (ts.isClassDeclaration(node)) {
                        const heritage = node.heritageClauses?.[0]
                        if (heritage) {
                            console.log(heritage.types?.[0].expression.getText());

                        }

                    }

                    /*
                    if (ts.isCallExpression(node) && node.expression.getText() === 'safely') {
                        const target = node.arguments[0]
                        if (ts.isPropertyAccessExpression(target)) {
                            return ts.createBinary(
                                target.expression,
                                ts.SyntaxKind.AmpersandAmpersandToken,
                                target
                            )
                        }
                    }
                    */
                    return ts.visitEachChild(node, visitor, ctx)
                }
                return ts.visitEachChild(sourceFile, visitor, ctx)
            }
        }
    }
}
