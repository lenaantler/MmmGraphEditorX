# MmmGraphEditor
GraphEditor Plugin for MikuMikuMoving(x64)



既存のMMM用グラフエディタプラグイン（https://ja.osdn.net/users/hmml/pf/mmm_graph_editor/wiki/FrontPage）を参考に適当に作った読み取り専用のグラフエディタです。

既存のグラフエディタプラグインには5461フレームまでしか表示できない仕様があったのをフルフレーム表示できるようにしてあります。

描画周りのコードもいくらか無駄を省いたので少しだけ軽いと思いますが、編集機能をそぎ落としたので移動や回転の滑らかさを視認するためだけのプラグインとなっています。

- 移動はグローバル座標系、回転はローカル座標系でのオイラー（ZXY）表現です。上下のスケールは自動伸縮で拡大縮小は（今のところ）できません。
- 移動XYZ回転XYZ各々を表示／非表示にできます。
- 既存グラフエディタプラグインと同じグラフ表示にはなりません。

## Install
MMM(x64)のPluginsフォルダにdllを配置してください。

MMM(x64)を起動し、プラグインを有効にすると選択したボーンのグラフが描画されます。

## Build 
1. cloneしたフォルダに#deployフォルダを作成し、その中にMMM(x64) ver 1.2.9.2を配置してください。
2. ビルドイベントでPluginsフォルダにバイナリがコピーされますが、MMMのフォルダ名が異なる場合は適当にビルドイベントを書き換えてください。
3. DxMath, MikuMiuPlugin, MikuMikuMoving64の参照は#deploy配下のものを指定してください。
4. デバッグは「外部プログラムの開始」で#deployの下に配置したMMM(x64)本体を指定してください。

