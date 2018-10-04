//Midtime Engine
//	本ソースコードのライセンスについては付属の LICENSE.html をお読みください。
//	Please read LICENSE.html for detail license information.


namespace MJHSC.MidtimeEngine.Plugins {

	/// <summary>
	/// Midtimeと言語プラグインとの接続
	/// </summary>
	public interface IMidtimeLanguage {

		/// <summary>
		/// 指定されたスクリプトがこの言語プラグインで処理可能かを確認し、その結果を返却します。
		/// 多くの場合、MidtimeResponse.OKしたスクリプトは次の手順でこの言語プラグインが処理することになります。
		/// できる限り拡張子の一致だけでなく、実際に処理可能なものか確認してください。
		/// </summary>
		/// <param name="ScriptFileName">テストするスクリプトファイルのパス（拡張子なし）</param>
		/// <returns>処理可能な場合: MidtimeResponse.OK
		/// 処理できない場合: MidtimeResponse.Error</returns>
		MidtimeResponse CanRunScript(string ScriptFileName);

		/// <summary>
		/// 新しいスクリプトがロードされるときに呼び出されます。
		/// 必要に応じて新しいインスタンスを作成し、実際にスクリプトを実行する関数を返却します。
		/// Midtime Engineはここで返却された関数をスクリプト実行スレッドで実行します。
		/// </summary>
		/// <param name="ScriptFileName"></param>
		/// <returns></returns>
		LanguageVMEntry StartVM(string ScriptFileName);

		/// <summary>
		/// 次のシーンに切り替わる直前に呼び出されます。リソースの開放などに使用します。
		/// 内部でSleepやUIの表示など遅延が発生する動作を行うと強制終了される場合があるので、
		/// 必要な解放処理のみを行い、即時returnしてください。
		/// </summary>
		/// <param name="ScriptFileName">実行中のスクリプトファイルのパス（拡張子なし）</param>
		/// <returns></returns>
		MidtimeResponse EndVM(string ScriptFileName);
		
	}
	
}